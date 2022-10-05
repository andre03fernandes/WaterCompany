using System.Collections.Generic;
using WaterCompany.Data.Entities;

namespace WaterCompany.Models
{
    public class UserViewModel : User
    {
        public string UserId { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}