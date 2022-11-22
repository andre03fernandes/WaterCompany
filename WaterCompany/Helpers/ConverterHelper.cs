using System;
using System.IO;
using WaterCompany.Data.Entities;
using WaterCompany.Models;
using static System.Net.Mime.MediaTypeNames;

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
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
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
                FirstName = client.FirstName,
                LastName = client.LastName,
                PhoneNumber = client.PhoneNumber,
                Email = client.Email,
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
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
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
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                PhoneNumber = employee.PhoneNumber,
                Email = employee.Email,
                Address = employee.Address,
                PostalCode = employee.PostalCode,
                TIN = employee.TIN,
                ImageId = employee.ImageId,
                User = employee.User
            };
        }

        public User ToUser(ChangeUserViewModel model, Guid imageId, bool isNew)
        {
            return new User
            {
                Id = isNew ? 0.ToString() : model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                Address = model.Address,
                PostalCode = model.PostalCode,
                TIN = model.TIN,
                ImageId = imageId
            };
        }

        public ChangeUserViewModel ToUserViewModel(User user)
        {
            return new ChangeUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Address = user.Address,
                PostalCode = user.PostalCode,
                TIN = user.TIN,
                ImageId = user.ImageId
            };
        }
    }
}