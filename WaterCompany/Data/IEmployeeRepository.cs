using System.Linq;
using WaterCompany.Data.Entities;

namespace WaterCompany.Data
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        public IQueryable GetAllWithUsers();
    }
}
