using Microsoft.EntityFrameworkCore;
using System.Linq;
using WaterCompany.Data.Entities;

namespace WaterCompany.Data
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly DataContext _context;

        public EmployeeRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Employees.Include(p => p.User).OrderBy(p => p.Id);
        }
    }
}
