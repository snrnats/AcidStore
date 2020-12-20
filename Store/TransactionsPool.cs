using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;

namespace Store
{
    public class TransactionsPool
    {
        private int _incrementTransactionId = 0;
        private ImmutableHashSet<int> _activeTransactions = ImmutableHashSet<int>.Empty;

        public Transaction CreateActive()
        {
            var transactionId = Interlocked.Increment(ref _incrementTransactionId);
            var transaction = new Transaction
            {
                Id = transactionId
            };
            _activeTransactions = _activeTransactions.Add(transactionId);
            return transaction;
        }

        public IReadOnlyCollection<int> ActiveTransactionIds()
        {
            return _activeTransactions;
        }

        public void Retain(Transaction transaction)
        {
            _activeTransactions = _activeTransactions.Remove(transaction.Id);
        }
    }
}
