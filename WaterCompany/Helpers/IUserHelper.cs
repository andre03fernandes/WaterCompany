using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using WaterCompany.Data.Entities;
using WaterCompany.Models;

namespace WaterCompany.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByUserNameAsync(string username);

        Task<IdentityResult> AddUserAsync(User user, string password);

        Task<SignInResult> LoginAsync(LoginViewModel model);

        Task LogoutAsync();
    }
}
