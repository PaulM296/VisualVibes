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
        public void Add(T entity)
        {
            if (_entities.Contains(entity))
            {
                Console.WriteLine($"Could not add the {nameof(T)}, because it already exists.");
                return;
            }
            _entities.Add(entity);
        }
        public async Task<T> AddAsync(T entity)
        {
            if (_entities.Contains(entity))
            {
                Console.WriteLine($"Could not add the {nameof(T)}, because it already exists.");
                await _logger.LogAsync(nameof(Add), isSuccess: false);
                return null;
            }
            _entities.Add(entity);
            await _logger.LogAsync(nameof(Add), isSuccess: true);
            return entity;
        }

        public ICollection<T> GetAll()
        {
            return _entities;
        }

        public async Task<ICollection<T>> GetAllAsync()
        {
            if (_entities.Count == 0)
            {
                await _logger.LogAsync(nameof(GetAll), isSuccess: false);
            }
            else
            {
                await _logger.LogAsync(nameof(GetAll), isSuccess: true);
            }
            return _entities;
        }

        public T GetById(Guid id)
        {
            var user = _entities.FirstOrDefault(e => e.Id == id);
            return user;
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


        public void Remove(T entity)
        {
            if (!_entities.Contains(entity))
            {
                Console.WriteLine("The entity does not exist, therefore it could not be removed.");
                return;
            }
            _entities.Remove(entity);
        }
        public async Task<T> RemoveAsync(T entity)
        {
            if (!_entities.Contains(entity))
            {
                Console.WriteLine("The entity does not exist, therefore it could not be removed.");
                await _logger.LogAsync(nameof(Remove), isSuccess: false);
                return null;
            }
            _entities.Remove(entity);
            await _logger.LogAsync(nameof(Remove), isSuccess: true);
            return entity;
        }

        public void Update(T updatedEntity)
        {
            int index = _entities.IndexOf(updatedEntity);
            _entities[index] = updatedEntity;

        }
        public async Task<T> UpdateAsync(T updatedEntity)
        {
            int index = _entities.IndexOf(updatedEntity);
            if (index == -1)
            {
                await _logger.LogAsync(nameof(Update), isSuccess: false);
            }
            else
            {
                _entities[index] = updatedEntity;
                await _logger.LogAsync(nameof(Update), isSuccess: true);
            }
            return _entities[index];
        }

    }
}
