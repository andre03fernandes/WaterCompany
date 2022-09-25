using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System;
using WaterCompany.Migrations;
using Microsoft.AspNetCore.Http;

namespace WaterCompany.Data.Entities
{
    public class User : IdentityUser
    {
        public string Name { get; set; }

        [Display(Name = "Image")]
        public Guid ImageId { get; set; }

        [Display(Name = "Image")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://nextlevel25853.blob.core.windows.net/images/noimage.png"
            : $"https://nextlevel25853.blob.core.windows.net/users/{ImageId}";
    }
}