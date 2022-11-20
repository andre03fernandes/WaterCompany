using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaterCompany.Data.Entities;

namespace WaterCompany.Data
{
    public interface IConsumptionRepository : IGenericRepository<Consumption>
    {
        IEnumerable<SelectListItem> GetComboClients();

        public Task<Consumption> GetConsumptionWithClients(int id);

        public IQueryable GetAllWithClients();

        Task<Client> GetClientsAsync(int id);

        Task DeleteConsumptionAsync(int id);
    }
}
