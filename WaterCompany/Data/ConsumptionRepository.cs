using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaterCompany.Data.Entities;

namespace WaterCompany.Data
{
    public class ConsumptionRepository : GenericRepository<Consumption>, IConsumptionRepository
    {
        private readonly DataContext _context;

        public ConsumptionRepository(DataContext context) : base(context)
        {
            _context = context;
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

        public async Task<Consumption> GetConsumptionWithClients(int id)
        {
            return await _context.Consumptions
                .Include(p => p.Client)
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }

        public IQueryable GetAllWithClients()
        {
            return _context.Consumptions.Include(p => p.Client).OrderBy(p => p.Id);
        }

        public async Task<Client> GetClientsAsync(int id)
        {
            return await _context.Clients.FindAsync(id);
        }

        public async Task DeleteConsumptionAsync(int id)
        {
            var consumption = await _context.Consumptions.FindAsync(id);
            if (consumption == null)
            {
                return;
            }

            _context.Consumptions.Remove(consumption);
            await _context.SaveChangesAsync();
        }
    }
}
