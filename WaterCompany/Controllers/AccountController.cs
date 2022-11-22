using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WaterCompany.Data;
using WaterCompany.Data.Entities;
using WaterCompany.Helpers;
using WaterCompany.Models;
using Response = WaterCompany.Helpers.Response;

namespace WaterCompany.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly ICountryRepository _countryRepository;
        private readonly IConfiguration _configuration;
        private readonly IMailHelper _mailHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IClientRepository _clientRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IConverterHelper _converterHelper;

        public AccountController(IUserHelper userHelper, ICountryRepository countryRepository,
            IConfiguration configuration, IMailHelper mailHelper, IBlobHelper blobHelper,
            RoleManager<IdentityRole> roleManager,
            IClientRepository clientRepository, IEmployeeRepository employeeRepository, IConverterHelper converterHelper)
        {
            _userHelper = userHelper;
            _countryRepository = countryRepository;
            _configuration = configuration;
            _mailHelper = mailHelper;
            _blobHelper = blobHelper;
            _roleManager = roleManager;
            _clientRepository = clientRepository;
            _employeeRepository = employeeRepository;
            _converterHelper = converterHelper;
        }

        //[Authorize(Roles = "Admin, Employee, Client")]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (this.Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(this.Request.Query["ReturnUrl"].First());
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            this.ModelState.AddModelError(string.Empty, "Failed to login!");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Register()
        {
            var list = _roleManager.Roles.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Select a role!",
                Value = "0"
            });

            var model = new RegisterNewUserViewModel
            {
                Roles = list,
                Countries = _countryRepository.GetComboCountries(),
                Cities = _countryRepository.GetComboCities(0),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByUserNameAsync(model.Username);
                if (user == null)
                {
                    var city = await _countryRepository.GetCityAsync(model.CityId);

                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        UserName = model.Username,
                        PhoneNumber = model.PhoneNumber,
                        CityId = model.CityId,
                        Address = model.Address,
                        TIN = model.TIN,
                        PostalCode = model.PostalCode,
                        City = city,
                    };

                    var result = await _userHelper.AddUserAsync(user, model.Password);
                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The user couldn't be created.");
                        return View(model);
                    }

                    var role = await _roleManager.FindByIdAsync(model.AccountRole);
                    if (role.Name == "Client")
                    {
                        Client client = new Client
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            Address = "To be filled in by the client",
                            PhoneNumber = model.PhoneNumber,
                            PostalCode = "0000-000",
                            TIN = "000000000",
                            ImageId = new Guid(),
                            User = user
                        };
                        await _userHelper.AddUserToRoleAsync(user, "Client");
                        await _clientRepository.CreateAsync(client);
                        await _userHelper.UpdateUserAsync(user);
                        var isInRole = await _userHelper.IsUserInRoleAsync(user, "Client");
                        if (!isInRole)
                        {
                            await _userHelper.AddUserToRoleAsync(user, "Client");
                        }

                    }
                    else if (role.Name == "Employee")
                    {
                        Employee employee = new Employee
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Email = model.Email,
                            Address = "To be filled in by the employee",
                            PhoneNumber = model.PhoneNumber,
                            PostalCode = "0000-000",
                            TIN = "000000000",
                            ImageId = new Guid(),
                            User = user
                        };

                        await _userHelper.AddUserToRoleAsync(user, "Employee");
                        await _employeeRepository.CreateAsync(employee);
                        await _userHelper.UpdateUserAsync(user);
                        var isInRole = await _userHelper.IsUserInRoleAsync(user, "Employee");
                        if (!isInRole)
                        {
                            await _userHelper.AddUserToRoleAsync(user, "Employee");
                        }
                    }



                }
                string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                string tokenLink = Url.Action("ConfirmEmail", "Account", new
                {
                    userid = user.Id,
                    token = myToken
                }, protocol: HttpContext.Request.Scheme);

                Response response = _mailHelper.SendEmail(model.Email, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                    $"To allow the user, " +
                    $"plase click in this link:</br></br><a href = \"{tokenLink}\"><b>Confirm Email</b></a>");


                if (response.IsSuccess)
                {
                    ViewBag.Message = "The instructions to allow you user has been sent to email";
                    return View(model);
                }

                ModelState.AddModelError(string.Empty, "The user couldn't be logged.");
            }

            return View(model);
        }

        public async Task<IActionResult> ChangeUser()
        {
            var user = await _userHelper.GetUserByUserNameAsync(this.User.Identity.Name);
            var model = new ChangeUserViewModel();

            if (user != null)
            {
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.PhoneNumber = user.PhoneNumber;
                model.Email = user.Email;
                model.Address = user.Address;
                model.ImageId = user.ImageId;
                model.TIN = user.TIN;
                model.PostalCode = user.PostalCode;
            }

            model = _converterHelper.ToUserViewModel(user);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model, User user)
        {
            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    Guid imageId = Guid.Empty;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "users");
                    }

                    user = _converterHelper.ToUser(model, imageId, false);
                    user = await _userHelper.GetUserByUserNameAsync(this.User.Identity.Name);

                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Email = model.Email;
                    user.Address = model.Address;
                    user.ImageId = imageId;
                    user.TIN = model.TIN;
                    user.PostalCode = model.PostalCode;

                    var response = await _userHelper.UpdateUserAsync(user);
                    if (response.Succeeded)
                    {
                        ViewBag.UserMessage = "The information of the boss was updated!";
                        return View(model);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                    }
                }
            }
            return View(model);
        }

        [Authorize(Roles = "Admin, Employee, Client")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByUserNameAsync(this.User.Identity.Name);

                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return this.RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User not found");
                }
            }
            return View(model);
        }

        [HttpPost]
        [Route("Account/GetCitiesAsync")]
        public async Task<JsonResult> GetCitiesAsync(int countryId)
        {
            var country = await _countryRepository.GetCountryWithCitiesAsync(countryId);
            return Json(country.Cities.OrderBy(c => c.Name));
        }

        [HttpPost] // Sempre post, não tem sentido fazer através do get
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model) // Recebe um modelo através do body (facultativo)
        {
            if (this.ModelState.IsValid) // se o modelo for válido
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username); // verificar se o email existe
                if (user != null) // se o email exisitir, verificamos a password (validação)
                {
                    var result = await _userHelper.ValidatePasswordAsync(
                        user,
                        model.Password);

                    if (result.Succeeded) // se a password for válida,criamos os claims
                    {
                        var claims = new[] // claims - onde estão as permissões, para criar os tokens, mecanismo que o middleware tem para criar os tokens
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email), // ao criar o token, ele cria uma zona em que vai registar o email do utilizador
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // e cria uma zona com um guid aleatório que fica associado ao email do utilizador criado antes
                        }; // mecanismo do middleware, conseguindo saber todo o processo da criação

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"])); // busca a key que está no "appsettings.json" do projeto e converte para bytes, ou seja, 0 e 1
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); // gera credenciais e agarra na key e gera o token, usando o algoritmo "HmacSha256"
                        var token = new JwtSecurityToken( // Jwt - um tipo de token
                            _configuration["Tokens:Issuer"], // issuer, audience, claims - parametros de configuração
                            _configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(30), // quando expira o token - neste caso o token é válido por 30 dias
                            signingCredentials: credentials);
                        var results = new // objeto anonimo
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token), // escreve o token criado anteriormente
                            expiration = token.ValidTo
                        };
                        return this.Created(string.Empty, results); // depois de criado manda um parametro vazio
                    }
                }
            }

            return BadRequest(); // se correr mal, manda um bad request
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {

            }
            return View();
        }

        public IActionResult RecoverPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user == null) // Existe user
                {
                    ModelState.AddModelError(string.Empty, "The email doesn't correspont to a registered user.");
                    return View(model);
                }

                var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user); // gera token
                var link = this.Url.Action( // cria Url para mudar a password
                    "ResetPassword",
                    "Account",
                    new { token = myToken }, protocol: HttpContext.Request.Scheme);

                Response response = _mailHelper.SendEmail(model.Email, "Shop Password Reset", $"<h1>Shop Password Reset</h1>" + // Manda o email
                $"To reset the password click in this link:</br></br>" +
                $"<a href = \"{link}\">Reset Password</a>");

                if (response.IsSuccess)
                {
                    this.ViewBag.Message = "The instructions to recover your password has been sent to email.";
                }
                return this.View();
            }
            return this.View(model);
        }

        public IActionResult ResetPassword(string token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userHelper.GetUserByUserNameAsync(model.UserName);
            if (user != null)
            {
                var result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    this.ViewBag.Message = "Password reset successful.";
                    return View();
                }
                this.ViewBag.Message = "Error while resetting the password.";
                return View(model);
            }
            this.ViewBag.Message = "User not found.";
            return View(model);
        }


        public IActionResult NotAuthorized()
        {
            return View();
        }
    }
}