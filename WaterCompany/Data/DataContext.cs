using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WaterCompany.Data.Entities;

namespace WaterCompany.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Client> Clients { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    }
}
