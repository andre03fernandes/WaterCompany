using System.Linq;
using System.Threading.Tasks;
using WaterCompany.Data.Entities;

namespace WaterCompany.Data
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        public IQueryable GetAllWithUsers();

        public IQueryable GetAllInvoices();

        Task<bool> ExistInvoiceConsumptionAsync(int id);

        Task<Client> GetClientsAsync(int id);
    }
}
