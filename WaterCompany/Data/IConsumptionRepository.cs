namespace WaterCompany.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using WaterCompany.Data.Entities;

    public interface IConsumptionRepository : IGenericRepository<Consumption>
    {
        IEnumerable<SelectListItem> GetComboClients();

        public Task<Consumption> GetConsumptionWithClients(int id);

        public IQueryable GetAllWithClients();

        Task<Client> GetClientsAsync(int id);

        Task DeleteConsumptionAsync(int id);

        public IQueryable GetAllByClient(int id);
    }
}