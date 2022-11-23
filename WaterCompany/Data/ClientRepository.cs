namespace WaterCompany.Data
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using WaterCompany.Data.Entities;

    public class ClientRepository : GenericRepository<Client>, IClientRepository
    {
        private readonly DataContext _context;

        public ClientRepository(DataContext context) : base(context) 
        {
            _context = context;
        }

        public IQueryable GetAllWithUsers()
        {
            return _context.Clients
                .Include(p => p.User)
                .OrderBy(p => p.Id);
        }

        public async Task<Client> GetClientByUserName(string username)
        {
            return await _context.Clients.Where(c => c.User.UserName == username).FirstOrDefaultAsync();
        }
    }
}