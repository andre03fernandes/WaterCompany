using System;
using System.IO;
using WaterCompany.Data.Entities;
using WaterCompany.Models;

namespace WaterCompany.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public Client ToClient(ClientViewModel model, Guid imageId, bool isNew)
        {
            return new Client
            {
                Id = isNew ? 0 : model.Id,
                ClientName = model.ClientName,
                Telephone = model.Telephone,
                Address = model.Address,
                PostalCode = model.PostalCode,
                TIN = model.TIN,
                Email = model.Email,
                ImageId = imageId,
                IsAvailable = model.IsAvailable,
                User = model.User
            };
        }

        public ClientViewModel ToClientViewModel(Client client)
        {
            return new ClientViewModel
            {
                Id = client.Id,
                ClientName = client.ClientName,
                Telephone = client.Telephone,
                Address = client.Address,
                PostalCode = client.PostalCode,
                TIN = client.TIN,
                Email = client.Email,
                ImageId = client.ImageId,
                IsAvailable = client.IsAvailable,
                User = client.User
            };
        }
    }
}
