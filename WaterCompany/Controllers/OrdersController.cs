using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Vereyon.Web;
using WaterCompany.Data;
using WaterCompany.Data.Entities;
using WaterCompany.Models;

namespace WaterCompany.Controllers
{
    [Authorize(Roles = ("Admin, Employee, Client"))]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly DataContext _context;
        private readonly IFlashMessage _flashMessage;

        public OrdersController(IOrderRepository orderRepository, DataContext context, IFlashMessage flashMessage)
        {
            _orderRepository = orderRepository;
            _context = context;
            _flashMessage = flashMessage;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _orderRepository.GetOrderAsync(this.User.Identity.Name);

            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = await _orderRepository.GetDetailTempsAsync(this.User.Identity.Name);
            return View(model);
        }

        public IActionResult AddOrder()
        {
            var model = new AddItemViewModel
            {
                Offers = _orderRepository.GetComboOffers()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder(AddItemViewModel model, OrderDetailTemp orderDetailTemp)
        {
            var offer = await _context.Offers.FindAsync(model.OfferId);
            if (offer.Name == "1st Echelon" && !(orderDetailTemp.Echelon <= 5 && orderDetailTemp.Echelon > 0))
            {
                ModelState.AddModelError(string.Empty, "The 1st Echelon only allows echelons up to 5m³");
            }
            else if(offer.Name == "2nd Echelon" && !(orderDetailTemp.Echelon > 5 && orderDetailTemp.Echelon <= 15))
            {
                ModelState.AddModelError(string.Empty, "The 2nd Echelon only allows echelons more than 5m³ and up to 15m³");
            }
            else if (offer.Name == "3rd Echelon" && !(orderDetailTemp.Echelon > 15 && orderDetailTemp.Echelon <= 25))
            {
                ModelState.AddModelError(string.Empty, "The 3rd Echelon only allows echelons more than 15m³ and up to 25m³");
            }
            else if (offer.Name == "4th Echelon" && !(orderDetailTemp.Echelon > 25))
            {
                ModelState.AddModelError(string.Empty, "The 4th Echelon only allows echelons more than 25m³");
            }
            else
            {
                await _orderRepository.AddItemToOrderAsync(model, this.User.Identity.Name);
                return RedirectToAction("Create");
            }
            return View(model);
        }
    }
}