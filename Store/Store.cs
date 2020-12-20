using System.Collections.Generic;
using System.Linq;

namespace Store
{
    public class Store<T> : IStore<T> where T : IIdentifiable
    {
        private readonly AtomicStore<T> _atomicStore;
        private readonly AtomicStore<TransactionHistory<T>> _transactionHistory;
        private readonly TransactionsPool _transactionsPool;

        public Store()
        {
            _atomicStore = new AtomicStore<T>();
            _transactionHistory = new AtomicStore<TransactionHistory<T>>();
            _transactionsPool = new TransactionsPool();
        }

        public void Create(T obj, Transaction transaction = null)
        {
            if (transaction != null)
            {
                _transactionHistory.Create(new TransactionHistory<T>()
                {
                    Id = transaction.Id,
                    Value = obj
                });
            }
            else
            {
                _atomicStore.Create(obj);
            }
        }

        public IReadOnlyCollection<T> GetAll(Transaction transaction = null)
        {
            return _atomicStore.GetAll();
        }

        public T Get(int id, Transaction transaction = null)
        {
            if (transaction != null)
            {
                var pendingValue = _transactionHistory.GetAll().OrderByDescending(x => x.TransactionId).FirstOrDefault(x => x.TransactionId == transaction.Id && x.Id == id);
                if (pendingValue != null)
                {
                    return pendingValue.Value;
                }
            }

            return _atomicStore.Get(id);
        }

        public void Update(T obj, Transaction transaction = null)
        {
            if (transaction != null)
            {
                _transactionHistory.Create(new TransactionHistory<T>()
                {
                    Id = transaction.Id,
                    Value = obj
                });
            }
            else
            {
                _atomicStore.Update(obj);
            }
        }

        public void Delete(int id, Transaction transaction = null)
        {
            _atomicStore.Delete(id);
        }

        public Transaction BeginTransaction()
        {
            return _transactionsPool.CreateActive();
        }

        public void Commit(Transaction transaction)
        {
            // apply history changes if there are no active transactions with lower id
            var activeTransaction = _transactionsPool.ActiveTransactionIds();
            if (activeTransaction.All(x => x >= transaction.Id))
            {
                var history = _transactionHistory.GetAll();
                var readyToApply = history.Where(x => x.TransactionId <= transaction.Id).ToArray();
                var latestChanges = readyToApply.OrderByDescending(x => x.TransactionId).GroupBy(x => x.Id).ToDictionary(x=> x.Key, grouping => grouping.FirstOrDefault());
                foreach (var (key, transactionHistory) in latestChanges)
                {
                    if (transactionHistory.Value != null)
                    {
                        var existing = _atomicStore.Get(key);
                        if (existing != null)
                        {
                            _atomicStore.Update(transactionHistory.Value);
                        }
                        else
                        {
                            _atomicStore.Create(transactionHistory.Value);
                        }
                    }
                    else
                    {
                        _atomicStore.Delete(transactionHistory.Id);
                    }
                }
            }

            _transactionsPool.Retain(transaction);
        }

        public void Abort(Transaction transaction)
        {
            var history = _transactionHistory.GetAll();
            foreach (var transactionHistory in history.Where(x=> x.TransactionId == transaction.Id))
            {
                _transactionHistory.Delete(transactionHistory.Id);
            }
            _transactionsPool.Retain(transaction);
        }
    }
}