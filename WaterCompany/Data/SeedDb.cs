namespace WaterCompany.Data
{
    using System;
    using System.Linq;
    using System.Security.Policy;
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

            var user = await _userHelper.GetUserByUserNameAsync("andre@admin");
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
                AddClient("Client", "1", user);
                AddClient("Client", "2", user);
                AddClient("Client", "3", user);
                await _context.SaveChangesAsync();
            }
        }

        private void AddClient(string firstname, string lastname, User user)
        {
            _context.Clients.Add(new Client
            {
                FirstName = firstname,
                LastName = lastname,
                Telephone = Convert.ToString(_random.Next(930000000, 939999999)),
                Address = "Rua xpto " + Convert.ToString(_random.Next(000, 999)),
                PostalCode = "1950-" + Convert.ToString(_random.Next(100, 999)),
                TIN = Convert.ToString(_random.Next(100000000, 999999999)),
                Email = "test" + Convert.ToString(_random.Next(000, 999)) + "@gmail.com",
                IsAvailable = true,
                User = user
            });
        }
    }
}