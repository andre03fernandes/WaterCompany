namespace WaterCompany.Data
{
    using System;
    using System.Collections.Generic;
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
            await _context.Database.EnsureCreatedAsync();

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Employee");
            await _userHelper.CheckRoleAsync("Client");

            if(!_context.Countries.Any())
            {
                var cities = new List<City>();
                cities.Add(new City { Name = "Lisboa" });
                cities.Add(new City { Name = "Porto" });
                cities.Add(new City { Name = "Faro" });

                _context.Countries.Add(new Country
                {
                    Cities = cities,
                    Name = "Portugal"
                });

                await _context.SaveChangesAsync();
            }

            var user = await _userHelper.GetUserByUserNameAsync("andre@admin");
            if(user == null)
            {
                user = new User
                {
                    FirstName = "André",
                    LastName = "Fernandes",
                    Email = "andre2411adm@gmail.com",
                    UserName = "andre@admin",
                    PhoneNumber = "927690241",
                    CityId = _context.Countries.FirstOrDefault().Cities.FirstOrDefault().Id,
                    City = _context.Countries.FirstOrDefault().Cities.FirstOrDefault()
                };

                var result = await _userHelper.AddUserAsync(user, "123456");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder!");
                }

                await _userHelper.AddUserToRoleAsync(user, "Admin");
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");
            if(!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }

            var user1 = await _userHelper.GetUserByUserNameAsync("staff@employee");
            if(user1 == null)
            {
                user1 = new User
                {
                    FirstName = "Staff",
                    LastName = "Employee",
                    Email = "staff@yopmail.com",
                    UserName = "staff@employee",
                    PhoneNumber = "927690241",
                    CityId = _context.Countries.FirstOrDefault().Cities.FirstOrDefault().Id,
                    City = _context.Countries.FirstOrDefault().Cities.FirstOrDefault()
                };

                var result1 = await _userHelper.AddUserAsync(user1, "123456");
                if (result1 != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder!");
                }

                await _userHelper.AddUserToRoleAsync(user1, "Employee");
                var token1 = await _userHelper.GenerateEmailConfirmationTokenAsync(user1);
                await _userHelper.ConfirmEmailAsync(user1, token1);
            }

            var isInRole1 = await _userHelper.IsUserInRoleAsync(user1, "Employee");
            if (!isInRole1)
            {
                await _userHelper.AddUserToRoleAsync(user1, "Employee");
            }

            if (!_context.Employees.Any())
            {
                AddEmployee(user1);
                await _context.SaveChangesAsync();
            }

            var user2 = await _userHelper.GetUserByUserNameAsync("roberto@client");
            if (user2 == null)
            {
                user2 = new User
                {
                    FirstName = "Roberto",
                    LastName = "Client",
                    Email = "client@yopmail.com",
                    UserName = "roberto@client",
                    PhoneNumber = "934567890",
                    CityId = _context.Countries.FirstOrDefault().Cities.FirstOrDefault().Id,
                    City = _context.Countries.FirstOrDefault().Cities.FirstOrDefault()
                };

                var result2 = await _userHelper.AddUserAsync(user2, "123456");
                if (result2 != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder!");
                }

                await _userHelper.AddUserToRoleAsync(user2, "Client");
                var token2 = await _userHelper.GenerateEmailConfirmationTokenAsync(user2);
                await _userHelper.ConfirmEmailAsync(user2, token2);
            }

            var isInRole2 = await _userHelper.IsUserInRoleAsync(user2, "Client");
            if (!isInRole2)
            {
                await _userHelper.AddUserToRoleAsync(user2, "Client");
            }

            if (!_context.Clients.Any())
            {
                AddClient(user2);
                await _context.SaveChangesAsync();
            }

        }

        private void AddEmployee(User user1)
        {
            _context.Employees.Add(new Employee
            {
                Address = "Rua otpx " + Convert.ToString(_random.Next(000, 999)),
                PostalCode = "2370-" + Convert.ToString(_random.Next(100, 999)),
                TIN = Convert.ToString(_random.Next(100000000, 999999999)),
                User = user1
            });
        }

        private void AddClient(User user)
        {
            _context.Clients.Add(new Client
            {
                Address = "Rua xpto " + Convert.ToString(_random.Next(000, 999)),
                PostalCode = "1950-" + Convert.ToString(_random.Next(100, 999)),
                TIN = Convert.ToString(_random.Next(100000000, 999999999)),
                User = user
            });
        }
    }
}