using Microsoft.EntityFrameworkCore;
using WaterCompany.Data.Entities;

namespace WaterCompany.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    }
}
