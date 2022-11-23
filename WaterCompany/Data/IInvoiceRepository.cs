namespace WaterCompany.Data
{
    using System.Linq;
    using System.Threading.Tasks;
    using WaterCompany.Data.Entities;

    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        public IQueryable GetAllWithUsers();

        public IQueryable GetAllInvoices();

        Task<bool> ExistInvoiceConsumptionAsync(int id);

        Task<Client> GetClientsAsync(int id);

        public IQueryable GetAllByClient(int id);

        Task<Invoice> GetLastInvoice();
    }
}