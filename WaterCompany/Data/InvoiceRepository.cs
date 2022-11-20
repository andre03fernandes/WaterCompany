using Microsoft.EntityFrameworkCore;
using System.Linq;
using WaterCompany.Data.Entities;

namespace WaterCompany.Data
{
    public class InvoiceRepository : GenericRepository<Invoice>, IInvoiceRepository
    {
        private readonly DataContext _context;

        public InvoiceRepository(DataContext context) : base(context)
        {
            _context = context;
        }
        public IQueryable GetAllWithUsers()
        {
            return _context.Invoices.Include(p => p.User);
        }
    }
}
