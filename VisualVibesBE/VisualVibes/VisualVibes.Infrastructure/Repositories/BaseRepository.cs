using Microsoft.EntityFrameworkCore;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;
using VisualVibes.Infrastructure.Exceptions;

namespace VisualVibes.Infrastructure.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly VisualVibesDbContext _context;

        public BaseRepository(VisualVibesDbContext context)
        {
            _context = context;
        }
       
        public async Task<T> AddAsync(T entity)
        {
            if (_context.Set<T>().Contains(entity))
            {
                throw new EntityAlreadyExistsException($"Could not add the {typeof(T).Name}, because it already exists.");
            }

            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();


            return entity;
        }

        public async Task<ICollection<T>> GetAllAsync()
        {
            var entities = await _context.Set<T>().ToListAsync();

            if (entities.Count == 0)
            {
                throw new EntityNotFoundException($"Could not find any {typeof(T).Name} entities.");
            }

            return entities;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var entity = await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);

            if (entity == null)
            {
                throw new EntityNotFoundException($"Could not find the specified {typeof(T).Name}."); ;
            }

            return entity;
        }

        public async Task<T> RemoveAsync(T entity)
        {
            var entityToRemove = await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == entity.Id);

            if (entityToRemove ==  null)
            {
                throw new EntityNotFoundException($"The {typeof(T).Name} does not exist, therefore it could not be removed.");
            }

            _context.Set<T>().Remove(entityToRemove);
            await _context.SaveChangesAsync();

            return entityToRemove;
        }

        public async Task<T> UpdateAsync(T updatedEntity)
        {
            var entity = await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == updatedEntity.Id);

            if (entity == null)
            {
                throw new EntityNotFoundException($"The {typeof(T).Name} has not been found, therefore it could not be removed.");
            }

            _context.Set<T>().Update(updatedEntity);
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
