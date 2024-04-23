using VisualVibes.App;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Infrastructure.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private static IList<T> _entities = new List<T>();
        private readonly FileSystemLogger _logger;

        public BaseRepository(FileSystemLogger logger)
        {
            _logger = logger;
        }
       
        public async Task<T> AddAsync(T entity)
        {
            if (_entities.Contains(entity))
            {
                Console.WriteLine($"Could not add the {nameof(T)}, because it already exists.");
                await _logger.LogAsync(nameof(AddAsync), isSuccess: false);
                return null;
            }
            _entities.Add(entity);
            await _logger.LogAsync(nameof(AddAsync), isSuccess: true);
            return entity;
        }

        public async Task<ICollection<T>> GetAllAsync()
        {
            if (_entities.Count == 0)
            {
                await _logger.LogAsync(nameof(GetAllAsync), isSuccess: false);
            }
            else
            {
                await _logger.LogAsync(nameof(GetAllAsync), isSuccess: true);
            }
            return _entities;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            var user = _entities.FirstOrDefault(e => e.Id == id);
            if (user == null)
            {
                await _logger.LogAsync(nameof(GetByIdAsync), isSuccess: false);
            }
            else
            {
                await _logger.LogAsync(nameof(GetByIdAsync), isSuccess: true);
            }
            return user;
        }

        public async Task<T> RemoveAsync(T entity)
        {
            if (!_entities.Contains(entity))
            {
                Console.WriteLine("The entity does not exist, therefore it could not be removed.");
                await _logger.LogAsync(nameof(RemoveAsync), isSuccess: false);
                return null;
            }
            _entities.Remove(entity);
            await _logger.LogAsync(nameof(RemoveAsync), isSuccess: true);
            return entity;
        }

        public async Task<T> UpdateAsync(T updatedEntity)
        {
            var entity = _entities.FirstOrDefault(e => e.Id == updatedEntity.Id);
            var index = _entities.IndexOf(entity);
            if (entity == null)
            {
                await _logger.LogAsync(nameof(UpdateAsync), isSuccess: false);
            }
            else
            {
                _entities[index] = updatedEntity;
                await _logger.LogAsync(nameof(UpdateAsync), isSuccess: true);
            }
            return _entities[index];
        }
    }
}
