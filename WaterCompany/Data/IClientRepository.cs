namespace WaterCompany.Data
{
    using System.Linq;
    using System.Threading.Tasks;
    using WaterCompany.Data.Entities;

    public interface IClientRepository : IGenericRepository<Client> 
    {
        public IQueryable GetAllWithUsers();

        Task<Client> GetClientByUserName(string username);
    }
}