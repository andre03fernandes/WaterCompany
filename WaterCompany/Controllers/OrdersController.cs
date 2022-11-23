namespace WaterCompany.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Vereyon.Web;
    using WaterCompany.Data;
    using WaterCompany.Data.Entities;
    using WaterCompany.Helpers;
    using WaterCompany.Models;

    [Authorize(Roles = ("Employee, Client"))]
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly DataContext _context;
        private readonly IFlashMessage _flashMessage;
        private readonly IUserHelper _userHelper;

        public OrdersController(IOrderRepository orderRepository, DataContext context, IFlashMessage flashMessage, IUserHelper userHelper)
        {
            _orderRepository = orderRepository;
            _context = context;
            _flashMessage = flashMessage;
            _userHelper = userHelper;
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
            else if (offer.Name == "2nd Echelon" && !(orderDetailTemp.Echelon > 5 && orderDetailTemp.Echelon <= 15))
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

        public async Task<IActionResult> DeleteItem(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("OrderNotFound");
            }
            await _orderRepository.DeleteDetailTempAsync(id.Value);
            return RedirectToAction("Create");
        }

        public async Task<IActionResult> DeleteOrder(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("OrderNotFound");
            }
            await _orderRepository.DeleteOrderAsync(id.Value);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ConfirmOrder()
        {
            var response = await _orderRepository.ConfirmOrderAsync(this.User.Identity.Name);
            if (response)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Create");
        }

        public async Task<IActionResult> Deliver(int? id)
        {
            if (id == null)
            {
                return new NotFoundViewResult("OrderNotFound");
            }

            var order = await _orderRepository.GetOrderAsync(id.Value);
            if (order == null)
            {
                return new NotFoundViewResult("OrderNotFound");
            }

            var model = new DeliveryViewModel
            {
                Id = order.Id,
                DeliveryDate = DateTime.Today
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Deliver(DeliveryViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _orderRepository.DeliverOrder(model);
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}