using System.Collections.Generic;

namespace Store
{
    public interface IStore<T> where T : IIdentifiable
    {
        void Create(T obj);
        IReadOnlyCollection<T> GetAll();
        T Get(int id);
        void Update(T obj);
        void Delete(int id);

        Transaction BeginTransaction();
        public void Commit(Transaction transaction);
        public void Abort(Transaction transaction);
    }
}