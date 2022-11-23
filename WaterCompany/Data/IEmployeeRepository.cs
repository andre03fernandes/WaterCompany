namespace WaterCompany.Data
{
    using System.Linq;
    using WaterCompany.Data.Entities;

    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        public IQueryable GetAllWithUsers();
    }
}