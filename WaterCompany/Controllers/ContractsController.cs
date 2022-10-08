using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

        // GET: Contracts
        public IActionResult Index()
        {
            return View(_contractRepository.GetAllWithClients());
        }

        // GET: Contracts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ContractNotFound");
            }

            var contract = await _contractRepository.GetByIdAsync(id.Value);
            if (contract == null)
            {
                return new NotFoundViewResult("ContractNotFound");
            }

            return View(contract);
        }

        // GET: Contracts/Create
        public IActionResult Create()
        {
            var model = new ContractViewModel
            {
                Clients = _contractRepository.GetComboClients(),
                //ContractTypes = _contractRepository.GetContractType(),
                //PaymentTypes = _contractRepository.GetPaymentType()
            };

            return View(model);
        }

        // POST: Contracts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContractViewModel model)
        {
            if (ModelState.IsValid)
            {
                var contract = await _contractRepository.GetByIdAsync(model.Id);
                if(contract == null)
                {
                    var client = await _contractRepository.GetClientsAsync(model.ClientId);
                    //var contractType = await _contractRepository.GetContractAsync(model.ContractTypeId);
                    //var paymentType = await _contractRepository.GetContractAsync(model.PaymentTypeId);

                    contract = new Contract
                    {
                        Client = client,
                        Address = model.Address,
                        PostalCode = model.PostalCode,
                        ContractDate = model.ContractDate,
                        //ContractType = contractType.ToString(),
                        //PaymentType = paymentType.ToString()
                    };
                }

                await _contractRepository.CreateAsync(contract);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Contracts/Edit/5
        public async Task<IActionResult> Edit(int? id, ContractViewModel model)
        {
            var contract = await _contractRepository.GetByIdAsync(id.Value);
            var client = await _clientRepository.GetByIdAsync(model.ClientId);

            if (id == null)
            {
                return NotFound();
            }

            if (contract == null)
            {
                return NotFound();
            }
            else
            {
                model.ClientId = Convert.ToInt32(client);
                model.Clients = _contractRepository.GetComboClients();
                model.Address = contract.Address;
                model.PostalCode = contract.PostalCode;
                model.ContractDate = contract.ContractDate;
            }

            model.Clients = _contractRepository.GetComboClients();
            return View(model);
        }

        // POST: Contracts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Contract contract)
        {
            if (id != contract.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _contractRepository.UpdateAsync(contract);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _contractRepository.ExistAsync(contract.Id))
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
            return View(contract);
        }

        // GET: Contracts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _contractRepository.GetByIdAsync(id.Value);
            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // POST: Contracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contract = await _contractRepository.GetByIdAsync(id);
            await _contractRepository.DeleteAsync(contract);
            return RedirectToAction(nameof(Index));
        }
    }
}
