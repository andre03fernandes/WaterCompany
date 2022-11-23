namespace WaterCompany.Helpers
{
    using System;
    using WaterCompany.Data.Entities;
    using WaterCompany.Models;

    public interface IConverterHelper
    {
        Client ToClient(ClientViewModel model, Guid imageId, bool isNew);

        ClientViewModel ToClientViewModel(Client client);

        Employee ToEmployee(EmployeeViewModel model, Guid imageId, bool isNew);

        EmployeeViewModel ToEmployeeViewModel(Employee employee);

        User ToUser(ChangeUserViewModel model, Guid imageId, bool isNew);

        ChangeUserViewModel ToUserViewModel(User user);
    }
}