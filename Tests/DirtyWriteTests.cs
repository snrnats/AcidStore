using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Store;

namespace Tests
{
    [TestClass]
    public class DirtyWriteTests
    {
        [TestMethod]
        public async Task Update_NewValueIsNotCommitted_OldValueIsRead()
        {
            const int firstValue = 200;
            const int secondValue = 300;
            const int id = 1;
            var store = new Store<Record>();
            store.Create(new Record(id, 100));

            var transaction = store.BeginTransaction();
            store.Update(new Record(id, firstValue), transaction);

            var secondUpdate = Task.Run(() => store.Update(new Record(id, secondValue)));

            var actual = store.Get(id);

            store.Commit(transaction);
            await secondUpdate;

            Assert.AreEqual(secondValue, actual.Balance);
        }

        [TestMethod]
        public async Task Update_NewValueWillBeAborted_OldValueIsRead()
        {
            const int firstValue = 200;
            const int secondValue = 300;
            const int id = 1;
            var store = new Store<Record>();
            store.Create(new Record(id, 100));

            var transaction = store.BeginTransaction();
            store.Update(new Record(id, firstValue), transaction);

            var secondUpdate = Task.Run(() => store.Update(new Record(id, secondValue)));

            var actual = store.Get(id);

            store.Abort(transaction);
            await secondUpdate;

            Assert.AreEqual(secondValue, actual.Balance);
        }
    }
}
