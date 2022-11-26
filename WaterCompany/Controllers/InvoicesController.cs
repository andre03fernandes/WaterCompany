namespace WaterCompany.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using WaterCompany.Data;
    using WaterCompany.Data.Entities;
    using WaterCompany.Helpers;

    public class InvoicesController : Controller
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IConsumptionRepository _consumptionRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IUserHelper _userHelper;

        public InvoicesController(
            IInvoiceRepository invoiceRepository,
            IConsumptionRepository consumptionRepository,
            IClientRepository clientRepository,
            IUserHelper userHelper)
        {
            _invoiceRepository = invoiceRepository;
            _consumptionRepository = consumptionRepository;
            _clientRepository = clientRepository;
            _userHelper = userHelper;
        }

        [Authorize(Roles = "Employee")]
        public IActionResult Index()
        {
            var invoices = _invoiceRepository.GetAllInvoices();
            return View(invoices);
        }

        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Edit(int? id)
        {
            var client = await _consumptionRepository.GetClientsAsync(id.Value);
            var invoices = await _invoiceRepository.GetByIdAsync(id.Value);
            var consumptions = await _consumptionRepository.GetConsumptionWithClients(id.Value);
            var model = new Invoice();
            if (invoices == null)
            {
                return NotFound();
            }
            else
            {
                model.IsPaid = invoices.IsPaid;
                model.InvoiceDate = invoices.InvoiceDate;
                model.Client = client;
                model.User = invoices.User;
                model.Consumption = consumptions;
                model.Total = invoices.Total;
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Invoice model)
        {
            if (ModelState.IsValid)
            {
                var client = await _consumptionRepository.GetClientsAsync(id.Value);
                var invoices = await _invoiceRepository.GetByIdAsync(id.Value);
                var consumptions = await _consumptionRepository.GetConsumptionWithClients(id.Value);
                if (invoices != null)
                {
                    invoices.IsPaid = model.IsPaid;
                    invoices.InvoiceDate = model.InvoiceDate;
                    invoices.User = model.User;
                    invoices.Client = client;
                    invoices.Consumption = consumptions;
                    invoices.Total = invoices.Total;


                    await _invoiceRepository.UpdateAsync(invoices);
                    return View(model);
                }
            }
            return View(model);
        }

        public IActionResult InvoiceNotFound()
        {
            return View();
        }

        [Authorize(Roles = "Client")]
        public async Task<IActionResult> InvoicesClient()
        {
            Client client = await _clientRepository.GetClientByUserName(this.User.Identity.Name);

            return View(_invoiceRepository.GetAllByClient(client.Id));
        }
    }
}