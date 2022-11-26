namespace WaterCompany.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using WaterCompany.Data.Entities;
    using WaterCompany.Helpers;
    using WaterCompany.Models;

    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public OrderRepository(DataContext context, IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task<IQueryable<Order>> GetOrderAsync(string userName)
        {
            var user = await _userHelper.GetUserByUserNameAsync(userName);
            if (user == null)
            {
                return null;
            }

            if (await _userHelper.IsUserInRoleAsync(user, "Employee"))
            {
                return _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.Items)
                    .ThenInclude(p => p.Offer)
                    .OrderByDescending(o => o.OrderDate);
            }

            return _context.Orders
                .Include(o => o.Items)
                .ThenInclude(p => p.Offer)
                .Where(o => o.User == user)
                .OrderByDescending(o => o.OrderDate);
        }

        public async Task<IQueryable<OrderDetailTemp>> GetDetailTempsAsync(string userName)
        {
            var user = await _userHelper.GetUserByUserNameAsync(userName);
            if (user == null)
            {
                return null;
            }

            return _context.OrderDetailsTemp
                .Include(p => p.Offer)
                .Where(o => o.User == user)
                .OrderBy(o => o.Offer.Name);
        }

        public IEnumerable<SelectListItem> GetComboOffers()
        {
            var list = _context.Offers.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Id.ToString()
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Select a offer!",
                Value = "0"
            });

            return list;
        }

        public async Task AddItemToOrderAsync(AddItemViewModel model, string userName)
        {
            var user = await _userHelper.GetUserByUserNameAsync(userName);
            if (user == null)
            {
                return;
            }

            var offer = await _context.Offers.FindAsync(model.OfferId);
            if (offer == null)
            {
                return;
            }

            var orderDetailTemp = await _context.OrderDetailsTemp
                .Where(odt => odt.User == user && odt.Offer == offer)
                .FirstOrDefaultAsync();

            if (orderDetailTemp == null)
            {

                orderDetailTemp = new OrderDetailTemp
                {
                    Offer = offer,
                    UnitaryValue = offer.UnitaryValue,
                    Echelon = model.Echelon,
                    User = user,
                };

                if (orderDetailTemp.Echelon <= 5 && orderDetailTemp.Echelon > 0)
                {
                    offer.UnitaryValue = 0.30;
                }
                else if (orderDetailTemp.Echelon > 5 && orderDetailTemp.Echelon <= 15)
                {
                    offer.UnitaryValue = 0.80;
                }
                else if (orderDetailTemp.Echelon > 15 && orderDetailTemp.Echelon <= 25)
                {
                    offer.UnitaryValue = 1.20;
                }
                else
                {
                    offer.UnitaryValue = 1.60;
                }

                _context.OrderDetailsTemp.Add(orderDetailTemp);
            }
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDetailTempAsync(int id)
        {
            var orderDetailTemp = await _context.OrderDetailsTemp.FindAsync(id);
            if (orderDetailTemp == null)
            {
                return;
            }

            _context.OrderDetailsTemp.Remove(orderDetailTemp);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _context.OrderDetails.FindAsync(id);
            if (order == null)
            {
                return;
            }

            _context.OrderDetails.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ConfirmOrderAsync(string userName)
        {
            var user = await _userHelper.GetUserByUserNameAsync(userName);
            if (user == null)
            {
                return false;
            }

            var orderTmps = await _context.OrderDetailsTemp
                .Include(o => o.Offer)
                .Where(o => o.User == user)
                .ToListAsync();

            if (orderTmps == null || orderTmps.Count == 0)
            {
                return false;
            }

            var details = orderTmps.Select(o => new OrderDetail
            {
                Echelon = o.Echelon,
                UnitaryValue = o.UnitaryValue,
            }).ToList();

            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                //DeliveryDate = DateTime.UtcNow,
                User = user,
                Items = details
            };

            await CreateAsync(order);
            _context.OrderDetailsTemp.RemoveRange(orderTmps);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task DeliverOrder(DeliveryViewModel model)
        {
            var order = await _context.Orders.FindAsync(model.Id);
            if (order == null)
            {
                return;
            }

            order.DeliveryDate = model.DeliveryDate;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetOrderAsync(int id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<Client> GetClientsAsync(int id)
        {
            return await _context.Clients.FindAsync(id);
        }

        public async Task<Order> GetConsumptionWithUsers(int id)
        {
            return await _context.Orders
                .Include(p => p.User)
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }
    }
}