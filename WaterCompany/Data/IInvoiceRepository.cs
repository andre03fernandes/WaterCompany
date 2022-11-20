using System.Linq;
using WaterCompany.Data.Entities;

namespace WaterCompany.Data
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        public IQueryable GetAllWithUsers();
    }
}
