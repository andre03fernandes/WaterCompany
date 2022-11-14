namespace WaterCompany.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
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

        public ClientsController(IClientRepository clientRepository,
            IUserHelper userHelper,
            IBlobHelper blobHelper,
            IConverterHelper converterHelper)
        {
            _clientRepository = clientRepository;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
            _converterHelper = converterHelper;
        }

        // GET: Clients
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View(_clientRepository.GetAllWithUsers());
        }

        // GET: Clients/Details/5
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

        // GET: Clients/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientViewModel model, User user)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "clients");
                }

                var client = _converterHelper.ToClient(user, model, imageId, true);

                // TODO: Modificar para o user que tiver logado
                client.User = await _userHelper.GetUserByIdAsync(User.Identity.Name);
                await _clientRepository.CreateAsync(client);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Clients/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
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

            var model = _converterHelper.ToClientViewModel(client);
            return View(model);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClientViewModel model, User user)
        {
            if (id != model.Id)
            {
                return new NotFoundViewResult("ClientNotFound");

            }

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

                    var clienteAntigo = await _clientRepository.GetByIdAsync(id);
                    user = await _userHelper.GetUserByEmailAsync(clienteAntigo.Email);

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
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Clients/Delete/5
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

        // POST: Clients/Delete/5
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
            catch(DbUpdateException ex)
            {
                if(id == null)
                {
                    return new NotFoundViewResult("ClientNotFound");
                }
                if(client == null)
                {
                    return new NotFoundViewResult("ClientNotFound");
                }
                if(ex.InnerException != null & ex.InnerException.Message.Contains("DELETE"))
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