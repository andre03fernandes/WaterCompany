using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaterCompany.Data.Entities;

namespace WaterCompany.Data
{
    public interface IContractRepository : IGenericRepository<Contract>
    {
        public IQueryable GetAllWithClients();

        IEnumerable<SelectListItem> GetComboClients();

        IEnumerable<SelectListItem> GetContractType();

        IEnumerable<SelectListItem> GetPaymentType();

        Task<Client> GetClientsAsync(int id);

        Task<Contract> GetContractAsync(int id);

        Task DeleteContractAsync(int id);

    }
}
