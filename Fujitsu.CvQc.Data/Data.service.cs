using Microsoft.EntityFrameworkCore;

namespace Fujitsu.CvQc.Data
{
    public abstract class DataService<T> : IDataService<T> where T : class
    {
        protected readonly DataContext dataContext;

        private static bool threadLocked = false;

        protected void WaitAndLockThread() {
            //TODO: Find an optimal way to avoid Entity Framework thread issue.  
            //      For now, this locking mecanism will insure that EF is used by only one thread at once.

            while (threadLocked) 
            {
                Thread.Sleep(10);
            }
            threadLocked = true;    
        }

        protected void UnlockThread() {
            //TODO: Find an optimal way to avoid Entity Framework thread issue.  
            //      For now, this locking mecanism will insure that EF is used by only one thread at once.

            threadLocked = false;
        }

        protected DataService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public T Add(T entity)
        {
            WaitAndLockThread();   
            var entities = dataContext.Set<T>();         
            entities.Add(entity);
            var task = dataContext.SaveChangesAsync();
            Task.WaitAll(task);
            UnlockThread();

            return entity;
        }

        public bool Delete(T entity)
        {
            WaitAndLockThread();
            var entities = dataContext.Set<T>();
            entities.Remove(entity);
            var task = dataContext.SaveChangesAsync();
            Task.WaitAll(task);
            var result = !entities.Contains(entity);
            UnlockThread();

            return result;
        }

        public T? Get(Guid id)
        {
            WaitAndLockThread();  
            var entities = dataContext.Set<T>();          
            var task = entities.FindAsync(id).AsTask();
            Task.WaitAll(task);  
            var result = task.Result;          
            UnlockThread();

            return result;
        }

        public List<T> GetAll()
        {
            WaitAndLockThread();
            var entities = dataContext.Set<T>();            
            var task = entities.ToListAsync();
            Task.WaitAll(task);
            var result = task.Result;          
           UnlockThread();

            return result;
        }

        public T Update(T entity)
        {
            WaitAndLockThread();  
            var entities = dataContext.Set<T>();          
            dataContext.Entry(entity).State = EntityState.Modified;
            var task = dataContext.SaveChangesAsync();
            Task.WaitAll(task);
            UnlockThread();

            return entity;
        }
    }
}
