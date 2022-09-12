using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using WaterCompany.Data.Entities;

namespace WaterCompany.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private Random _random;

        public SeedDb(DataContext context)
        {
            _context = context;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();

            if (!_context.Clients.Any())
            {
                AddClient("Ricardo Simão");
                AddClient("José Simão");
                AddClient("Ricardo Mourato");
                await _context.SaveChangesAsync();
            }
        }

        private void AddClient(string name)
        {
            _context.Clients.Add(new Client
            {
                ClientName = name,
                Telephone = Convert.ToString(_random.Next(930000000, 939999999)),
                Address = "Rua xpto",
                PostalCode = "1950-345",
                TIN = Convert.ToString(_random.Next(000000000, 999999999)),
                Email = "test1@gmail.com",
                IsAvailable = true
            });
        }
    }
}
