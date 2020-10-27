using System;
using System.Collections.Generic;

namespace Store
{
    public class Store<T> : IStore<T> where T : IIdentifiable
    {
        private readonly AtomicStore<T> _atomicStore;

        public Store()
        {
            _atomicStore = new AtomicStore<T>();
        }

        public void Create(T obj, Transaction transaction = null)
        {
            _atomicStore.Create(obj);
        }

        public IReadOnlyCollection<T> GetAll(Transaction transaction = null)
        {
            return _atomicStore.GetAll();
        }

        public T Get(int id, Transaction transaction = null)
        {
            return _atomicStore.Get(id);
        }

        public void Update(T obj, Transaction transaction = null)
        {
            _atomicStore.Update(obj);
        }

        public void Delete(int id, Transaction transaction = null)
        {
            _atomicStore.Delete(id);
        }

        public Transaction BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public void Commit(Transaction transaction)
        {
            throw new NotImplementedException();
        }

        public void Abort(Transaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}