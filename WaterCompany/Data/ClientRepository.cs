using WaterCompany.Data.Entities;

namespace WaterCompany.Data
{
    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        public ClientRepository(DataContext context) : base(context)
        {
                
        }
    }
}
