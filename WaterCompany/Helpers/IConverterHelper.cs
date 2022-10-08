﻿using System;
using WaterCompany.Data.Entities;
using WaterCompany.Models;

namespace WaterCompany.Helpers
{
    public interface IConverterHelper
    {
        Client ToClient(User user, ClientViewModel model, Guid imageId, bool isNew);

        ClientViewModel ToClientViewModel(Client client);

        Employee ToEmployee(User user, EmployeeViewModel model, Guid imageId, bool isNew);

        EmployeeViewModel ToEmployeeViewModel(Employee employee);
    }
}