using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using WaterCompany.Data.Entities;

namespace WaterCompany.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByUserNameAsync(string username);

        Task<IdentityResult> AddUserAsync(User user, string password);
    }
}
