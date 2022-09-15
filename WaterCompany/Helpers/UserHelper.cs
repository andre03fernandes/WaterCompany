﻿namespace WaterCompany.Helpers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Identity;
    using WaterCompany.Data.Entities;

    public class UserHelper : IUserHelper
    {
        private readonly UserManager<User> _userManager;

        public UserHelper(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<User> GetUserByUserNameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }
    }
}