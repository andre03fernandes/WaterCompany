namespace WaterCompany.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using WaterCompany.Data;
    using WaterCompany.Data.Entities;
    using WaterCompany.Helpers;

    public class ContactUsController : Controller
    {
        private readonly DataContext _context;
        private readonly IMailHelper _mailHelper;

        public ContactUsController(DataContext context, IMailHelper mailHelper)
        {
            _context = context;
            _mailHelper = mailHelper;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactUs(ContactUs contactUs)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contactUs);
                await _context.SaveChangesAsync();
                Response response = _mailHelper.SendEmail("andre2411adm@gmail.com", contactUs.Subject, "Name: " + contactUs.Name + "<br /><br />" + "Email: " + contactUs.Email + "<br /><br />" + "Phone Number: " + contactUs.PhoneNumber + "<br /><br />" + "Message: " + contactUs.Message);
                if (response.IsSuccess)
                {
                    ViewBag.Message = "Wait for the next level staff contact";
                }
            }
            return View("ContactUs");
        }
    }
}