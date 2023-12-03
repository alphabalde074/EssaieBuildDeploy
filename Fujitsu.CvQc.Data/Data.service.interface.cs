namespace Fujitsu.CvQc.Data
{
    public interface IDataService<T>
    {
        public List<T> GetAll();
        public T? Get(Guid id);
        public T Add(T entity);
        public T Update(T entity);
        public bool Delete(T entity);
    }
}
