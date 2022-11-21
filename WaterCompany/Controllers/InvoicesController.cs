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

namespace WaterCompany.Controllers
{
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

        public IActionResult Index()
        {
            var invoices = _invoiceRepository.GetAllInvoices();
            return View(invoices);
        }
    }
}
