namespace VisualVibes.App.Interfaces
{
    public interface IRepository<T>
    {
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        IList<T> GetAll();
        T GetById(int id);

    }
}
