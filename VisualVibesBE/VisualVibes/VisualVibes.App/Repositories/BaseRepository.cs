using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : BaseEntity
    {
        public void Add(T entity)
        {
            throw new NotImplementedException();
        }

        public IList<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public T GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(T entity)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
