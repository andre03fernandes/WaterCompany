namespace WaterCompany.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using WaterCompany.Data.Entities;
    using WaterCompany.Helpers;

    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private Random _random;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync();

            var user = await _userHelper.GetUserByEmailAsync("andre2411adm@gmail.com");
            if(user == null)
            {
                user = new User
                {
                    FirstName = "André",
                    LastName = "Fernandes",
                    Email = "andre2411adm@gmail.com",
                    UserName = "andre@admin",
                    PhoneNumber = "927690241"
                };

                var result = await _userHelper.AddUserAsync(user, "123456");
                if(result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder!");
                }
            }

            if (!_context.Clients.Any())
            {
                AddClient("Ricardo Simão", user);
                AddClient("José Simão", user);
                AddClient("Ricardo Mourato", user);
                await _context.SaveChangesAsync();
            }
        }

        private void AddClient(string name, User user)
        {
            _context.Clients.Add(new Client
            {
                ClientName = name,
                Telephone = Convert.ToString(_random.Next(930000000, 939999999)),
                Address = "Rua xpto",
                PostalCode = "1950-345",
                TIN = Convert.ToString(_random.Next(000000000, 999999999)),
                Email = "test1@gmail.com",
                IsAvailable = true,
                User = user
            });
        }
    }
}