using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaterCompany.Data.Entities;

namespace WaterCompany.Data
{
    public class ContractRepository : GenericRepository<Contract>, IContractRepository
    {
        private readonly DataContext _context;

        public ContractRepository(DataContext context) : base(context)
        {
            _context = context;
        }
        public IQueryable GetAllWithClients()
        {
            return _context.Contracts.Include(p => p.Client).OrderBy(p => p.Id);
        }

        public async Task<Client> GetClientsAsync(int id)
        {
            return await _context.Clients.FindAsync(id);
        }
        public async Task<Contract> GetContractAsync(int id)
        {
            return await _context.Contracts.FindAsync(id);
        }


        public IEnumerable<SelectListItem> GetComboClients()
        {
            var list = _context.Clients.Select(c => new SelectListItem
            {
                Text = c.FirstName + " " + c.LastName,
                Value = c.Id.ToString()

            }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Select a client!",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetContractType()
        {
            var list = _context.Contracts.Select(c => new SelectListItem
            {
                //Text = c.ContractType,
                Value = c.Id.ToString()

            }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Select a contract type!",
                Value= "0"
            });

            list.Insert(1, new SelectListItem
            {
                Text = "Domestic",
                Value = "1"
            });

            list.Insert(2, new SelectListItem
            {
                Text = "Agricultural",
                Value = "2"
            });

            list.Insert(3, new SelectListItem
            {
                Text = "Business",
                Value = "3"
            });

            list.Insert(4, new SelectListItem
            {
                Text = "Industrial",
                Value = "4"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetPaymentType()
        {
            var list = _context.Contracts.Select(c => new SelectListItem
            {
                //Text = c.PaymentType,
                Value = c.Id.ToString()

            }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "Select a payment type!",
                Value = "0"
            });

            list.Insert(1, new SelectListItem
            {
                Text = "ATM",
                Value = "1"
            });

            list.Insert(2, new SelectListItem
            {
                Text = "MB Way",
                Value = "2"
            });

            list.Insert(3, new SelectListItem
            {
                Text = "PayPal",
                Value = "3"
            });

            list.Insert(4, new SelectListItem
            {
                Text = "Bank Transfer",
                Value = "4"
            });

            return list;
        }

        public async Task<Contract> GetContractWithClients(int id)
        {
            return await _context.Contracts
                .Include(p => p.Client)
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task DeleteContractAsync(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if(contract == null)
            {
                return;
            }

            _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();
        }
    }
}
