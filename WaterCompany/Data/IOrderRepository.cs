using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaterCompany.Data.Entities;
using WaterCompany.Models;

namespace WaterCompany.Data
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<IQueryable<Order>> GetOrderAsync(string userName);

        Task<IQueryable<OrderDetailTemp>> GetDetailTempsAsync(string userName);

        IEnumerable<SelectListItem> GetComboOffers();

        Task AddItemToOrderAsync(AddItemViewModel model, string userName);

        Task DeleteDetailTempAsync(int id);
    }
}
