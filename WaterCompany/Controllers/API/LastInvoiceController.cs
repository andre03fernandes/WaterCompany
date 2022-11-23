namespace WaterCompany.Controllers.API
{
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using WaterCompany.Data;

    [Route("api/[controller]")]
    [ApiController]
    public class LastInvoiceController : Controller
    {
        private readonly IInvoiceRepository _invoiceRepository;

        public LastInvoiceController(IInvoiceRepository invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetLastInvoice()
        {
            return Ok(await _invoiceRepository.GetLastInvoice());
        }
    }
}