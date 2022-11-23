namespace WaterCompany.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
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

            if (!_context.Countries.Any())
            {
                var cities = new List<City>();
                cities.Add(new City { Name = "Lisboa" });
                cities.Add(new City { Name = "Porto" });
                cities.Add(new City { Name = "Faro" });
                cities.Add(new City { Name = "Viseu" });
                cities.Add(new City { Name = "Aveiro" });
                cities.Add(new City { Name = "Alentejo" });
                cities.Add(new City { Name = "Almada" });
                cities.Add(new City { Name = "Amadora" });
                cities.Add(new City { Name = "Braga" });

                _context.Countries.Add(new Country
                {
                    Cities = cities,
                    Name = "Portugal"
                });

                await _context.SaveChangesAsync();
            }

            var user = await _userHelper.GetUserByUserNameAsync("andre@admin");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "André",
                    LastName = "Fernandes",
                    Email = "andre2411adm@gmail.com",
                    UserName = "andre@admin",
                    PhoneNumber = "927690241",
                    CityId = _context.Countries.FirstOrDefault().Cities.FirstOrDefault().Id,
                    City = _context.Countries.FirstOrDefault().Cities.FirstOrDefault(),
                    Address = "Rua Vale Formoso de Cima 113 5ºD",
                    PostalCode = "1950-266",
                    TIN = "245960368"
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
            if (!isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }

            var user1 = await _userHelper.GetUserByUserNameAsync("staff@employee");
            if (user1 == null)
            {
                user1 = new User
                {
                    FirstName = "Staff",
                    LastName = "Employee",
                    Email = "staff@yopmail.com",
                    UserName = "staff@employee",
                    PhoneNumber = "927690241",
                    CityId = _context.Countries.FirstOrDefault().Cities.FirstOrDefault().Id,
                    City = _context.Countries.FirstOrDefault().Cities.FirstOrDefault(),
                    Address = "Rua xpto 1",
                    PostalCode = "1970-456",
                    TIN = "123435675"
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
                    City = _context.Countries.FirstOrDefault().Cities.FirstOrDefault(),
                    Address = "Rua xpto 2",
                    PostalCode = "2456-345",
                    TIN = "678567345"
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

            if (!_context.Offers.Any())
            {
                AddOffer("4th Echelon", "More than 25m³", 1.60, user);
                AddOffer("3rd Echelon", "More than 15m³ and up to 25m³", 1.20, user);
                AddOffer("2nd Echelon", "More than 5m³ and up to 15m³", 0.80, user);
                AddOffer("1st Echelon", "Up to 5m³", 0.30, user);
                await _context.SaveChangesAsync();
            }
        }

        private void AddOffer(string name, string echelonlimit, double unitaryValue, User user)
        {
            _context.Offers.Add(new Offer
            {
                Name = name,
                EchelonLimit = echelonlimit,
                UnitaryValue = unitaryValue,
                IsAvailable = true,
                User = user
            });
        }

        private void AddEmployee(User user1)
        {
            _context.Employees.Add(new Employee
            {
                FirstName = user1.FirstName,
                LastName = user1.LastName,
                PhoneNumber = user1.PhoneNumber,
                Email = user1.Email,
                Address = user1.Address,
                PostalCode = user1.PostalCode,
                TIN = user1.TIN,
                User = user1
            });
        }

        private void AddClient(User user)
        {
            _context.Clients.Add(new Client
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Address = user.Address,
                PostalCode = user.PostalCode,
                TIN = user.TIN,
                User = user
            });
        }
    }
}