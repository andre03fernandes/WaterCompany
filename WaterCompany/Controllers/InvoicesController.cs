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