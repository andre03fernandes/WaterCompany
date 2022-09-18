using System;
using WaterCompany.Data.Entities;
using WaterCompany.Models;

namespace WaterCompany.Helpers
{
    public interface IConverterHelper
    {
        Client ToClient(ClientViewModel model, Guid imageId, bool isNew);

        ClientViewModel ToClientViewModel(Client client);
    }
}