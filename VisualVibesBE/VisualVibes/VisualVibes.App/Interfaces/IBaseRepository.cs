using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(Guid id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> RemoveAsync(T entity);
        Task<ICollection<T>> GetAllAsync();

    }
}
