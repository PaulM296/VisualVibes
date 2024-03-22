using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private static IList<T> _entities = new List<T>();
        public void Add(T entity)
        {
            if(_entities.Contains(entity))
            {
                Console.WriteLine($"Could not add the {nameof(T)}, because it already exists.");
                return;
            }
            _entities.Add(entity);
        }

        public ICollection<T> GetAll()
        {
            return _entities;
        }

        public T GetById(Guid id)
        {
            return _entities.FirstOrDefault(e => e.Id == id);
        }

        public void Remove(T entity)
        {
            if(!_entities.Contains(entity))
            {
                Console.WriteLine("The entity does not exist, therefore it could not be removed.");
                return;
            }
            _entities.Remove(entity);
        }

        public void Update(T updatedEntity)
        {
            int index = _entities.IndexOf(updatedEntity);
            _entities[index] = updatedEntity;
        }
    }
}
