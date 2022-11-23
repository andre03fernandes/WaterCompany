namespace WaterCompany.Models
{
    using System.Collections.Generic;
    using WaterCompany.Data.Entities;

    public class UserViewModel : User
    {
        public string UserId { get; set; }

        public IEnumerable<string> Roles { get; set; }
    }
}