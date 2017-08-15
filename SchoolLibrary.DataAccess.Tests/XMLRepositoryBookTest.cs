using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchoolLibrary.DataAccess.Repository;
using SchoolLibrary.Common.Helpers;
using SchoolLibrary.DataAccess.Entity;
using System.Collections.Generic;

namespace SchoolLibrary.DataAccess.Tests
{
    [TestClass]
    public class XMLRepositoryBookTest
    {
        IRepository<Book> Repository { get; set; }
        public object HostingEnvironment { get; private set; }

        [TestInitialize]
        public void Initialize()
        {
            Repository = new XMLRepository<Book>(ConfigHelper.BooksFilePath);
        }

        [TestMethod]
        public void FirstDeleteAllTest()
        {
            IList<Book> entities = Repository.Get();
            int count = entities.Count;
            for (int iIndex = count - 1; iIndex >= 0; iIndex--)
                Repository.Delete(entities[iIndex].ID);

            entities = Repository.Get();

            Assert.AreEqual(0, entities.Count);
        }

        [TestMethod]
        public void InsertTest()
        {
            IList<Book> entities = Repository.Get();
            int count = entities.Count;
            for (int iIndex = count - 1; iIndex >= 0; iIndex--)
                Repository.Delete(entities[iIndex].ID);

            Repository.Insert(new Book() { ID = 1, Name = "B1", StudentName = "" });
            entities = Repository.Get();

            Assert.AreEqual(1, entities.Count);
            Assert.AreEqual(1, entities[0].ID);
        }

        [TestMethod]
        public void UpdateTest()
        {
            IList<Book> entities = Repository.Get();
            int count = entities.Count;
            for (int iIndex = count - 1; iIndex >= 0; iIndex--)
                Repository.Delete(entities[iIndex].ID);
            Repository.Insert(new Book() { ID = 1, Name = "B1", StudentName = "" });

            Repository.Update(new Book() { ID = 1, Name = "B2", StudentName = "SN1" });
            entities = Repository.Get();

            Assert.AreEqual(1, entities.Count);
            Assert.AreEqual("B2", entities[0].Name);
        }

        [TestMethod]
        public void GetListTest()
        {
            IList<Book> entities = Repository.Get();
            int count = entities.Count;
            for (int iIndex = count - 1; iIndex >= 0; iIndex--)
                Repository.Delete(entities[iIndex].ID);

            Repository.Insert(new Book() { ID = 1, Name = "B1", StudentName = "" });
            Repository.Insert(new Book() { ID = 2, Name = "B2", StudentName = "" });
            Repository.Insert(new Book() { ID = 3, Name = "B3", StudentName = "" });

            entities = Repository.Get();

            Assert.AreEqual(3, entities.Count);
            Assert.AreEqual("B1", entities[0].Name);
            Assert.AreEqual("B2", entities[1].Name);
            Assert.AreEqual("B3", entities[2].Name);
        }

        [TestMethod]
        public void LastDeleteAllTest()
        {
            IList<Book> entities = Repository.Get();
            int count = entities.Count;
            for (int iIndex = count - 1; iIndex >= 0; iIndex--)
                Repository.Delete(entities[iIndex].ID);

            entities = Repository.Get();

            Assert.AreEqual(0, entities.Count);
        }
    }
}
