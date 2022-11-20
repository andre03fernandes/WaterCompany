namespace WaterCompany.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Vereyon.Web;
    using WaterCompany.Data;
    using WaterCompany.Data.Entities;
    using WaterCompany.Helpers;
    using WaterCompany.Models;

    public class ClientsController : Controller
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly UserManager<User> _userManager;
        private readonly DataContext _context;
        private readonly IFlashMessage _flashMessage;

        public ClientsController(IClientRepository clientRepository,
            IUserHelper userHelper,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper, UserManager<User> userManager, DataContext context, IFlashMessage flashMessage)
        {
            _clientRepository = clientRepository;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
            _userManager = userManager;
            _context = context;
            _flashMessage = flashMessage;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View(_clientRepository.GetAllWithUsers());
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ClientNotFound");
            }

            var client = await _clientRepository.GetByIdAsync(id.Value);
            if (client == null)
            {
                return new NotFoundViewResult("ClientNotFound");
            }

            return View(client);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (this.User.IsInRole("Client"))
            {
                var thisUser = await _userManager.GetUserAsync(HttpContext.User);
                var userId = thisUser.Id;
                var clientId = _context.Clients.Include(u => u.User).Where(u => u.User == thisUser).Select(u => u.Id).Single();
                id = clientId;
            }

            if (id == null)
            {
                return new NotFoundViewResult("ClientNotFound");
            }

            var client = await _clientRepository.GetByIdAsync(id.Value);
            if (client == null)
            {
                return new NotFoundViewResult("ClientNotFound");
            }

            var model = _converterHelper.ToClientViewModel(client);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClientViewModel model, User user)
        {
            if (id != model.Id)
            {
                return new NotFoundViewResult("ClientNotFound");

            }

            var clientID = await _clientRepository.GetByIdAsync(model.Id);

            if (ModelState.IsValid)
            {
                try
                {
                    Guid imageId = Guid.Empty;

                    if (model.ImageFile != null && model.ImageFile.Length > 0)
                    {
                        imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "clients");
                    }

                    var client = _converterHelper.ToClient(user, model, imageId, false);
                    model.ImageId = imageId;

                    //var clienteAntigo = await _clientRepository.GetByIdAsync();
                    user = await _userHelper.GetUserByEmailAsync(model.Email);

                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                    user.PhoneNumber = model.PhoneNumber;
                    user.ImageId = model.ImageId;

                    await _userHelper.UpdateUserAsync(user);
                    await _clientRepository.UpdateAsync(model);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _clientRepository.ExistAsync(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                if (this.User.IsInRole("Admin"))
                {
                    return RedirectToAction(nameof(Index));
                }
                if (this.User.IsInRole("Client"))
                {
                    ViewBag.ClientMessage = "The information of this client was updated!";
                }
            }
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ClientNotFound");
            }

            var client = await _clientRepository.GetByIdAsync(id.Value);
            if (client == null)
            {
                return new NotFoundViewResult("ClientNotFound");
            }

            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var client = await _clientRepository.GetByIdAsync(id.Value);
            try
            {
                await _clientRepository.DeleteAsync(client);
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (id == null)
                {
                    return new NotFoundViewResult("ClientNotFound");
                }
                if (client == null)
                {
                    return new NotFoundViewResult("ClientNotFound");
                }
                if (ex.InnerException != null & ex.InnerException.Message.Contains("DELETE"))
                {
                    ViewBag.ErrorTitle = $"The client {client.FullName} has at least one associated contract";
                    ViewBag.ErrorMessage = $"The client <b>{client.FullName}</b> cannot be erased because it has at least one associated contract. <br /><br />" +
                        $"If you want to delete this client you must first delete all the contracts associated with it and then try again to delete it.";
                }
                return View("Error");
            }
        }

        public IActionResult ClientNotFound()
        {
            return View();
        }
    }
}