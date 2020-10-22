using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Store;

namespace Tests
{
    [TestClass]
    public class StoreTests
    {
        private static readonly Record[] SavedRecords = {new Record(100, 0), new Record(101, 0), new Record(102, 0)};

        [TestMethod]
        public void Create_EmptyStore_ObjectIsSaved()
        {
            const int id = 1;
            var store = CreateEmptyStore();

            var savedRecord = new Record(id, 500);
            store.Create(savedRecord);

            var loadedRecord = store.Get(id);
            Assert.AreEqual(loadedRecord.Id, savedRecord.Id);
        }

        [TestMethod]
        public void Get_RecordInStore_RecordIsRetrieved()
        {
            var store = CreateFilledStore();

            var loadedRecord = store.Get(SavedRecords[0].Id);

            Assert.IsNotNull(loadedRecord);
            Assert.AreEqual(SavedRecords[0], loadedRecord);
        }

        [TestMethod]
        public void GetAll_RecordsInStore_RecordsAreRetrieved()
        {
            var store = CreateFilledStore();

            var loadedRecords = store.GetAll();

            Assert.IsNotNull(loadedRecords);
            CollectionAssert.AreEquivalent(SavedRecords, loadedRecords.ToArray());
        }

        [TestMethod]
        public void Update_RecordsInStore_RecordIsUpdated()
        {
            var store = CreateFilledStore();

            var uploadedRecord = new Record(100, 500);
            store.Update(uploadedRecord);

            var loadedRecord = store.Get(100);
            Assert.AreEqual(uploadedRecord, loadedRecord);
        }

        [TestMethod]
        public void Delete_RecordsInStore_RecordIsDeleted()
        {
            var store = CreateFilledStore();

            store.Delete(100);

            var loadedRecord = store.Get(100);
            Assert.IsNull(loadedRecord);
        }

        private static Store<Record> CreateEmptyStore()
        {
            return new Store<Record>();
        }

        private static Store<Record> CreateFilledStore()
        {
            var store = CreateEmptyStore();
            foreach (var savedRecord in SavedRecords)
            {
                store.Create(savedRecord);
            }

            return store;
        }
    }
}