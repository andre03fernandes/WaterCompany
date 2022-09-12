using System.Linq;
using System.Threading.Tasks;

namespace WaterCompany.Data
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll(); // Método que devolve todas as entidades (a que o T tiver a usar) (IQueryable - lista)

        Task<T> GetByIdAsync(int id);

        Task CreateAsync(T entity); // Parâmetro - entidade T - genérico

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task<bool> ExistAsync(int id); // Se existe uma entidade que recebe o id
    }
}