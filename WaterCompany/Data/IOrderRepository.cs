namespace WaterCompany.Data
{
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using WaterCompany.Data.Entities;
    using WaterCompany.Models;

    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IQueryable<Order>> GetOrderAsync(string userName);

        Task<IQueryable<OrderDetailTemp>> GetDetailTempsAsync(string userName);

        IEnumerable<SelectListItem> GetComboOffers();

        Task AddItemToOrderAsync(AddItemViewModel model, string userName);

        Task DeleteDetailTempAsync(int id);

        Task<bool> ConfirmOrderAsync(string userName);

        Task DeliverOrder(DeliveryViewModel model);

        Task<Order> GetOrderAsync(int id);

        public Task<Client> GetClientsAsync(int id);

        public Task<Order> GetConsumptionWithUsers(int id);

        Task DeleteOrderAsync(int id);
    }
}