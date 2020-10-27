using System.Collections.Generic;

namespace Store
{
    public interface IStore<T> where T : IIdentifiable
    {
        void Create(T obj, Transaction transaction = null);
        IReadOnlyCollection<T> GetAll(Transaction transaction = null);
        T Get(int id, Transaction transaction = null);
        void Update(T obj, Transaction transaction = null);
        void Delete(int id, Transaction transaction = null);

        Transaction BeginTransaction();
        public void Commit(Transaction transaction);
        public void Abort(Transaction transaction);
    }
}