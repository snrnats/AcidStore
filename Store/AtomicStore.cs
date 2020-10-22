using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Store
{
    public class AtomicStore<T> where T : IIdentifiable
    {
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();
        private readonly List<T> _store = new List<T>();

        public void Create(T obj)
        {
            _lock.EnterWriteLock();
            try
            {
                _store.Add(obj);
            }
            finally
            {
                _lock.ExitWriteLock();
            }
        }

        public IReadOnlyCollection<T> GetAll()
        {
            _lock.EnterReadLock();
            try
            {
                return _store;
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public T Get(int id)
        {
            _lock.EnterReadLock();
            try
            {
                return _store.FirstOrDefault(x => x.Id == id);
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }

        public void Update(T obj)
        {
            _lock.EnterUpgradeableReadLock();
            try
            {
                var index = _store.FindIndex(x => x.Id == obj.Id);
                if (index != -1)
                {
                    _lock.EnterWriteLock();
                    try
                    {
                        _store[index] = obj;
                    }
                    finally
                    {
                        _lock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                _lock.ExitUpgradeableReadLock();
            }
        }

        public void Delete(int id)
        {
            _lock.EnterUpgradeableReadLock();
            try
            {
                var index = _store.FindIndex(x => x.Id == id);
                if (index != -1)
                {
                    _lock.EnterWriteLock();
                    try
                    {
                        _store.RemoveAt(index);
                    }
                    finally
                    {
                        _lock.ExitWriteLock();
                    }
                }
            }
            finally
            {
                _lock.ExitUpgradeableReadLock();
            }
        }
    }
}