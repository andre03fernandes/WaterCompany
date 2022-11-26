namespace WaterCompany.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using WaterCompany.Data;
    using WaterCompany.Data.Entities;
    using WaterCompany.Helpers;
    using WaterCompany.Models;

    public class ConsumptionsController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IConsumptionRepository _consumptionRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IMailHelper _mailHelper;

        public ConsumptionsController(
                IUserHelper userHelper,
                IConsumptionRepository consumptionRepository,
                IInvoiceRepository invoiceRepository,
                IClientRepository clientRepository,
                IContractRepository contractRepository, IMailHelper mailHelper)
        {
            _consumptionRepository = consumptionRepository;
            _userHelper = userHelper;
            _invoiceRepository = invoiceRepository;
            _clientRepository = clientRepository;
            _contractRepository = contractRepository;
            _mailHelper = mailHelper;
        }

        [Authorize(Roles = "Employee")]
        public IActionResult Index()
        {
            var consumptions = _consumptionRepository.GetAllWithClients();
            return View(consumptions);
        }

        [Authorize(Roles = "Employee")]
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

        [Authorize(Roles = "Employee, Client")] // Permissão para o cliente inserir um consumo durante os últimos 15 dias de cada mês, após isso essa permissão é retirada
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

        [Authorize(Roles = "Employee")]
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

        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Delete(int? id)
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

        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> GenerateInvoice(int? id)
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
                model.Client = client;
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
        public async Task<IActionResult> GenerateInvoice(int id, ConsumptionViewModel view)
        {
            var consumption = await _consumptionRepository.GetConsumptionWithClients(id);
            var client = await _consumptionRepository.GetClientsAsync(id);

            view.Client = client;
            view.Clients = _consumptionRepository.GetComboClients();
            view.ClientId = consumption.Client.Id;
            view.Echelon = consumption.Echelon;
            view.ConsumptionDate = consumption.ConsumptionDate;
            view.UnitaryValue = consumption.UnitaryValue;
            view.TotalConsumption = consumption.TotalConsumption;

            bool InvoiceExist = await _invoiceRepository.ExistInvoiceConsumptionAsync(id);
            if (InvoiceExist)
            {
                ModelState.AddModelError(string.Empty, "There is already an invoice for this consumption!");
                return View(view);
            }

            try
            {
                Invoice invoice = new Invoice
                {
                    InvoiceDate = DateTime.Now,
                    Total = view.TotalConsumption
                };

                await _invoiceRepository.CreateAsync(invoice);

                invoice.Client = client;
                invoice.Consumption = consumption;
                invoice.Consumption.Echelon = consumption.Echelon;
                invoice.Consumption.UnitaryValue = consumption.UnitaryValue;
                invoice.User = await _userHelper.GetUserByUserNameAsync(this.User.Identity.Name);

                await _invoiceRepository.UpdateAsync(invoice);

                Response response = _mailHelper.SendEmail(client.Email, "New Invoice", "A new invoice has been made available");
                if (response.IsSuccess)
                {
                    return RedirectToAction("Index", "Invoices");
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _consumptionRepository.ExistAsync(view.Id))
                {
                    return new NotFoundViewResult("ConsumptionNotFound");
                }
                else
                {
                    throw;
                }
            }
            return View(view);
        }

        [Authorize(Roles = "Client")]
        public async Task<IActionResult> CreateByClient()
        {
            Client client = await _clientRepository.GetClientByUserName(this.User.Identity.Name);
            var model = new ConsumptionViewModel
            {
                Client = client,
                ClientId = client.Id,
                Clients = _consumptionRepository.GetComboClients()
            };
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateByClient(ConsumptionViewModel model)
        {
            Client client = await _clientRepository.GetClientByUserName(this.User.Identity.Name);
            var user = await _userHelper.GetUserByUserNameAsync(this.User.Identity.Name);
            model.ClientId = client.Id;

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
                return RedirectToAction(nameof(ClientConsumptions));
        }

        public async Task<IActionResult> ClientConsumptions()
        {
            Client client = await _clientRepository.GetClientByUserName(this.User.Identity.Name);
            return View(_consumptionRepository.GetAllByClient(client.Id));
        }
    }
}