using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WaterCompany.Data;
using WaterCompany.Data.Entities;
using WaterCompany.Helpers;
using WaterCompany.Models;

namespace WaterCompany.Controllers
{
    public class ConsumptionsController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IConsumptionRepository _consumptionRepository;
        private readonly IInvoiceRepository _invoiceRepository;

        public ConsumptionsController(
                IUserHelper userHelper,
                IConsumptionRepository consumptionRepository,
                IInvoiceRepository invoiceRepository)
        {
            _consumptionRepository = consumptionRepository;
            _userHelper = userHelper;
            _invoiceRepository = invoiceRepository;
        }

        public IActionResult Index()
        {
            var consumptions = _consumptionRepository.GetAllWithClients();
            return View(consumptions);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("ConsumptionNotFound");

            }

            var consumption = await _consumptionRepository.GetConsumptionWithClients(id.Value);
            if (consumption == null)
            {
                return new NotFoundViewResult("ConsumptionNotFound");
            }

            return View(consumption);
        }

        [Authorize(Roles = "Employee")]
        public IActionResult Create()
        {
            var model = new ConsumptionViewModel
            {
                Clients = _consumptionRepository.GetComboClients(),
            };

            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(ConsumptionViewModel model)
        {
            var client = await _consumptionRepository.GetClientsAsync(model.ClientId);
            if (ModelState.IsValid)
            {
                int echelon = model.Echelon;
                double unitaryValue = model.UnitaryValue;
                double totalConsumption = model.TotalConsumption;

                if (echelon >= 5)
                {
                    echelon -= 5;
                    unitaryValue = 0.30;
                    totalConsumption = 5 * unitaryValue;

                    if (echelon >= 10)
                    {
                        echelon -= 10;
                        unitaryValue = 0.80;
                        totalConsumption += 10 * unitaryValue;

                        if (echelon >= 10)
                        {
                            echelon -= 10;
                            unitaryValue = 1.20;
                            totalConsumption += 10 * unitaryValue;
                            unitaryValue = 1.60;
                            totalConsumption += echelon * unitaryValue;
                        }
                        else
                        {
                            unitaryValue = 1.20;
                            totalConsumption += echelon * unitaryValue;
                        }
                    }
                    else
                    {
                        unitaryValue = 0.80;
                        totalConsumption += echelon * unitaryValue;
                    }
                }
                else
                {
                    unitaryValue = 0.30;
                    totalConsumption = (echelon * unitaryValue);
                }
                Consumption consumption = new Consumption
                {
                    Client = client,
                    ConsumptionDate = model.ConsumptionDate,
                    Echelon = model.Echelon,
                    UnitaryValue = unitaryValue,
                    TotalConsumption = totalConsumption
                };
                await _consumptionRepository.CreateAsync(consumption);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var client = await _consumptionRepository.GetClientsAsync(id.Value);
            var consumptions = await _consumptionRepository.GetConsumptionWithClients(id.Value);
            var model = new ConsumptionViewModel();
            if (consumptions == null)
            {
                return new NotFoundViewResult("ConsumptionNotFound");
            }
            else
            {
                model.ClientId = client.Id;
                model.Clients = _consumptionRepository.GetComboClients();
                model.UnitaryValue = consumptions.UnitaryValue;
                model.ConsumptionDate = consumptions.ConsumptionDate;
                model.TotalConsumption = consumptions.TotalConsumption;
                model.Echelon = consumptions.Echelon;
            }
            model.Clients = _consumptionRepository.GetComboClients();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ConsumptionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var consumption = await _consumptionRepository.GetConsumptionWithClients(id);
                if (consumption != null)
                {
                    var client = await _consumptionRepository.GetClientsAsync(model.ClientId);
                    int echelon = model.Echelon;
                    double unitaryValue = model.UnitaryValue;
                    double totalConsumption = model.TotalConsumption;

                    if (echelon >= 5)
                    {
                        echelon -= 5;
                        unitaryValue = 0.30;
                        totalConsumption = 5 * unitaryValue;

                        if (echelon >= 10)
                        {
                            echelon -= 10;
                            unitaryValue = 0.80;
                            totalConsumption += 10 * unitaryValue;

                            if (echelon >= 10)
                            {
                                echelon -= 10;
                                unitaryValue = 1.20;
                                totalConsumption += 10 * unitaryValue;
                                unitaryValue = 1.60;
                                totalConsumption += echelon * unitaryValue;
                            }
                            else
                            {
                                unitaryValue = 1.20;
                                totalConsumption += echelon * unitaryValue;
                            }
                        }
                        else
                        {
                            unitaryValue = 0.80;
                            totalConsumption += echelon * unitaryValue;
                        }
                    }
                    else
                    {
                        unitaryValue = 0.30;
                        totalConsumption = (echelon * unitaryValue);
                    }
                    model.TotalConsumption = totalConsumption;

                    consumption.Client = client;
                    consumption.ConsumptionDate = model.ConsumptionDate;
                    consumption.UnitaryValue = unitaryValue;
                    consumption.TotalConsumption = totalConsumption;
                    consumption.Echelon = model.Echelon;

                    await _consumptionRepository.UpdateAsync(consumption);
                    var allConsumptions = _consumptionRepository.GetAllWithClients();
                    return View("Index", allConsumptions);
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

            var consumption = await _consumptionRepository.GetConsumptionWithClients(id.Value);
            if (consumption == null)
            {
                return NotFound();
            }

            return View(consumption);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var consumption = await _consumptionRepository.GetByIdAsync(id);
            await _consumptionRepository.DeleteAsync(consumption);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ConsumptionNotFound()
        {
            return View();
        }
    }
}
