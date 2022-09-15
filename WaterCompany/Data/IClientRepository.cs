﻿using System.Linq;
using WaterCompany.Data.Entities;

namespace WaterCompany.Data
{
    public interface IClientRepository : IGenericRepository<Client> 
    {
        public IQueryable GetAllWithUsers();
    }

}