using Microsoft.EntityFrameworkCore;
using VisualVibes.App;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;
using VisualVibes.Infrastructure.Exceptions;

namespace VisualVibes.Infrastructure.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly VisualVibesDbContext _context;
        protected readonly FileSystemLogger _logger;

        public BaseRepository(VisualVibesDbContext context, FileSystemLogger logger)
        {
            _context = context;
            _logger = logger;
        }
       
        public async Task<T> AddAsync(T entity)
        {
            if (_context.Set<T>().Contains(entity))
            {
                await _logger.LogAsync(nameof(AddAsync), isSuccess: false);
                throw new EntityAlreadyExistsException($"Could not add the {nameof(T)}, because it already exists.");
            }

            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();

            await _logger.LogAsync(nameof(AddAsync), isSuccess: true);

            return entity;
        }

        public async Task<ICollection<T>> GetAllAsync()
        {
            var entities = await _context.Set<T>().ToListAsync();

            if (entities.Count == 0)
            {
                await _logger.LogAsync(nameof(GetAllAsync), isSuccess: false);
            }
            else
            {
                await _logger.LogAsync(nameof(GetAllAsync), isSuccess: true);
            }

            return entities;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var entity = await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);

            if (entity == null)
            {
                await _logger.LogAsync(nameof(GetByIdAsync), isSuccess: false);
            }
            else
            {
                await _logger.LogAsync(nameof(GetByIdAsync), isSuccess: true);
            }

            return entity;
        }

        public async Task<T> RemoveAsync(T entity)
        {
            var entityToRemove = await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == entity.Id);

            if (entityToRemove ==  null)
            {
                await _logger.LogAsync(nameof(RemoveAsync), isSuccess: false);
                throw new EntityNotFoundException($"The {nameof(T)} does not exist, therefore it could not be removed.");
            }

            _context.Set<T>().Remove(entityToRemove);
            await _context.SaveChangesAsync();

            await _logger.LogAsync(nameof(RemoveAsync), isSuccess: true);

            return entityToRemove;
        }

        public async Task<T> UpdateAsync(T updatedEntity)
        {
            var entity = await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == updatedEntity.Id);

            if (entity == null)
            {
                await _logger.LogAsync(nameof(UpdateAsync), isSuccess: false);
                throw new EntityNotFoundException($"The {nameof(T)} has not been found, therefore it could not be removed.");
            }

            _context.Set<T>().Update(updatedEntity);
            await _context.SaveChangesAsync();

            await _logger.LogAsync(nameof(UpdateAsync), isSuccess: true);
            return entity;
        }
    }
}
