namespace VisualVibes.App.Interfaces
{
    public interface IBaseRepository<T>
    {
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        ICollection<T> GetAll();
        T GetById(Guid id);

    }
}
