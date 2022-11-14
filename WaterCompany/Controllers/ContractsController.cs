using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WaterCompany.Data;
using WaterCompany.Data.Entities;
using WaterCompany.Helpers;
using WaterCompany.Models;

namespace WaterCompany.Controllers
{
    public class ContractsController : Controller
    {
        private readonly IContractRepository _contractRepository;
        private readonly IClientRepository _clientRepository;

        public ContractsController(IContractRepository contractRepository, IClientRepository clientRepository)
        {
            _contractRepository = contractRepository;
            _clientRepository = clientRepository;
        }

        public IActionResult Index()
        {
            var contracts = _contractRepository.GetAllWithClients();
            return View(contracts);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ContractNotFound");
            }

            var contract = await _contractRepository.GetContractWithClients(id.Value);
            if (contract == null)
            {
                return new NotFoundViewResult("ContractNotFound");
            }

            return View(contract);
        }

        public IActionResult Create()
        {
            var model = new ContractViewModel
            {
                Clients = _contractRepository.GetComboClients(),
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContractViewModel model)
        {
            var client = await _contractRepository.GetClientsAsync(model.ClientId);
            if (ModelState.IsValid)
            {
                try
                {
                    Contract contract = new Contract
                    {
                        Client = client,
                        Address = model.Address,
                        ContractDate = model.ContractDate,
                        PostalCode = model.PostalCode,
                    };

                    await _contractRepository.CreateAsync(contract);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.InnerException.Message);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var contracts = await _contractRepository.GetContractWithClients(id.Value);
            var model = new ContractViewModel();
            if (contracts == null)
            {
                return new NotFoundViewResult("ContractsNotFound");
            }
            else
            {
                model.ClientId = contracts.Client.Id;
                model.Clients = _contractRepository.GetComboClients();
                model.Address = contracts.Address;
                model.PostalCode = contracts.PostalCode;
                model.ContractDate = contracts.ContractDate;
            }

            model.Clients = _contractRepository.GetComboClients();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, ContractViewModel model)
        {
            if (ModelState.IsValid)
            {
                var contract = await _contractRepository.GetContractWithClients(id.Value);
                if (contract != null)
                {
                    var client = await _contractRepository.GetClientsAsync(model.ClientId);

                    contract.Client = client;
                    contract.Address = model.Address;
                    contract.PostalCode = model.PostalCode;
                    contract.ContractDate = model.ContractDate;

                    await _contractRepository.UpdateAsync(contract);
                    var allContracts = _contractRepository.GetAllWithClients();
                    return View("Index", allContracts);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _contractRepository.GetContractWithClients(id.Value);
            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contract = await _contractRepository.GetByIdAsync(id);
            await _contractRepository.DeleteAsync(contract);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ContractNotFound()
        {
            return View();
        }
    }
}
