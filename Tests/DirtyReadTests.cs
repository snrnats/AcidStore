using Microsoft.VisualStudio.TestTools.UnitTesting;
using Store;

namespace Tests
{
    [TestClass]
    public class DirtyReadTests
    {
        [TestMethod]
        public void Get_NewValueIsNotCommitted_OldValueIsRead()
        {
            const int oldValue = 100;
            const int newValue = 200;
            const int id = 1;
            var store = new Store<Record>();
            store.Create(new Record(id, oldValue));

            var transaction = store.BeginTransaction();
            store.Update(new Record(id, newValue), transaction);

            var actual = store.Get(id);

            store.Commit(transaction);

            Assert.AreEqual(oldValue, actual.Balance);
        }

        [TestMethod]
        public void Get_NewValueWillBeAborted_OldValueIsRead()
        {
            const int oldValue = 100;
            const int newValue = 200;
            const int id = 1;
            var store = new Store<Record>();
            store.Create(new Record(id, oldValue));

            var transaction = store.BeginTransaction();
            store.Update(new Record(id, newValue), transaction);

            var actual = store.Get(id);

            store.Abort(transaction);

            Assert.AreEqual(oldValue, actual.Balance);
        }
    }
}
