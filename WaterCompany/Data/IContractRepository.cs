﻿namespace WaterCompany.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using WaterCompany.Data.Entities;

    public interface IContractRepository : IGenericRepository<Contract>
    {
        public IQueryable GetAllWithClients();

        IEnumerable<SelectListItem> GetComboClients();

        IEnumerable<SelectListItem> GetContractType();

        IEnumerable<SelectListItem> GetPaymentType();

        Task<Client> GetClientsAsync(int id);

        Task<Contract> GetContractAsync(int id);

        Task DeleteContractAsync(int id);

        public Task<Contract> GetContractWithClients(int id);
    }
}