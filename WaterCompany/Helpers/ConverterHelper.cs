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
                Address = model.Address,
                PostalCode = model.PostalCode,
                TIN = model.TIN,
                ImageId = imageId,
                User = model.User
            };
        }

        public ClientViewModel ToClientViewModel(Client client)
        {
            return new ClientViewModel
            {
                Id = client.Id,
                Address = client.Address,
                PostalCode = client.PostalCode,
                TIN = client.TIN,
                ImageId = client.ImageId,
                User = client.User
            };
        }

        public Employee ToEmployee(EmployeeViewModel model, Guid imageId, bool isNew)
        {
            return new Employee
            {
                Id = isNew ? 0 : model.Id,
                Address = model.Address,
                PostalCode = model.PostalCode,
                TIN = model.TIN,
                ImageId = imageId,
                User = model.User
            };
        }

        public EmployeeViewModel ToEmployeeViewModel(Employee employee)
        {
            return new EmployeeViewModel
            {
                Id = employee.Id,
                Address = employee.Address,
                PostalCode = employee.PostalCode,
                TIN = employee.TIN,
                ImageId = employee.ImageId,
                User = employee.User
            };
        }
    }
}
