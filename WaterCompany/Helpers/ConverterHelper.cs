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
                FirstName = model.FirstName,
                LastName = model.LastName,
                Telephone = model.Telephone,
                Address = model.Address,
                PostalCode = model.PostalCode,
                TIN = model.TIN,
                Email = model.Email,
                ImageId = imageId,
                User = model.User
            };
        }

        public ClientViewModel ToClientViewModel(Client client)
        {
            return new ClientViewModel
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Telephone = client.Telephone,
                Address = client.Address,
                PostalCode = client.PostalCode,
                TIN = client.TIN,
                Email = client.Email,
                ImageId = client.ImageId,
                User = client.User
            };
        }

        public Employee ToEmployee(EmployeeViewModel model, Guid imageId, bool isNew)
        {
            return new Employee
            {
                Id = isNew ? 0 : model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Telephone = model.Telephone,
                Address = model.Address,
                PostalCode = model.PostalCode,
                TIN = model.TIN,
                Email = model.Email,
                ImageId = imageId,
                User = model.User
            };
        }

        public EmployeeViewModel ToEmployeeViewModel(Employee employee)
        {
            return new EmployeeViewModel
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Telephone = employee.Telephone,
                Address = employee.Address,
                PostalCode = employee.PostalCode,
                TIN = employee.TIN,
                Email = employee.Email,
                ImageId = employee.ImageId,
                User = employee.User
            };
        }
    }
}
