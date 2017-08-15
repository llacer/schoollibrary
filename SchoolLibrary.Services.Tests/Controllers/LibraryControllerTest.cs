using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SchoolLibrary.Common;
using SchoolLibrary.Common.Helpers;
using SchoolLibrary.DataAccess.Entity;
using SchoolLibrary.DataAccess.Repository;
using SchoolLibrary.Services;
using SchoolLibrary.Services.Controllers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;

namespace SchoolLibrary.Services.Tests.Controllers
{
    [TestClass]
    public class LibraryControllerTest
    {
        private IRepository<Message> MessageRepository { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            MessageRepository = new XMLRepository<Message>(ConfigHelper.MessagesFilePath);
            IEnumerable<Message> messages = MessageRepository.Get();
        }

        [TestMethod]
        public void GetEmptyTest()
        {
            // Arrange
            IList<Book> entities = null;
            Mock<IRepository<Book>> frMock = new Mock<IRepository<Book>>();
            frMock.Setup(fr => fr.Get()).Returns(entities);

            LibraryController controller = new LibraryController(frMock.Object, MessageRepository);

            // Act
            IList<Book> books = controller.Get();

            // Assert
            Assert.IsNull(books);
        }

        [TestMethod]
        public void GetTest()
        {
            // Arrange
            IList<Book> entities = new List<Book>();
            entities.Add(new Book() { ID = 1, Name = "Book One", StudentName = "" });
            entities.Add(new Book() { ID = 2, Name = "Book Two", StudentName = "" });
            entities.Add(new Book() { ID = 3, Name = "Book Three", StudentName = "" });

            Mock<IRepository<Book>> frMock = new Mock<IRepository<Book>>();
            frMock.Setup(fr => fr.Get()).Returns(entities);

            LibraryController controller = new LibraryController(frMock.Object, MessageRepository);

            // Act
            IList<Book> books = controller.Get();

            // Assert
            Assert.IsNotNull(books);
            Assert.AreEqual(3, books.Count);
            Assert.AreEqual("Book One", books[0].Name);
            Assert.AreEqual("", books[0].StudentName);
        }
        
        [TestMethod]
        public void AssignValidationFailTest()
        {
            // Arrange
            IList<Book> entities = null;
            Mock<IRepository<Book>> frMock = new Mock<IRepository<Book>>();
            frMock.Setup(fr => fr.Get()).Returns(entities);

            LibraryController controller = new LibraryController(frMock.Object, MessageRepository);
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());

            // Act
            ObjectContent objContent = controller.Assign(0, "").Content as ObjectContent;
            ResponseMessage responseMessage = objContent.Value as ResponseMessage;

            // Assert
            Assert.AreEqual(false, responseMessage.Success);
        }

        [TestMethod]
        public void AssignFailTest()
        {
            // Arrange
            Book book = new Book() { ID = 1, Name = "Book One", StudentName = "SN1", ReturnDate = DateTime.Now.ToShortDateString() };
            Mock<IRepository<Book>> frMock = new Mock<IRepository<Book>>();
            frMock.Setup(fr => fr.Find(It.IsAny<int>())).Returns(book);

            LibraryController controller = new LibraryController(frMock.Object, MessageRepository);
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());

            // Act
            ObjectContent objContent = controller.Assign(1, "SN1").Content as ObjectContent;
            ResponseMessage responseMessage = objContent.Value as ResponseMessage;

            // Assert
            Assert.AreEqual(false, responseMessage.Success);
            Assert.AreEqual("Book is already borrowed.", responseMessage.Message);
        }

        [TestMethod]
        public void AssignTest()
        {
            // Arrange
            Book book = new Book() { ID = 1, Name = "Book One", StudentName = "", ReturnDate = "" };
            Mock<IRepository<Book>> frMock = new Mock<IRepository<Book>>();
            frMock.Setup(fr => fr.Find(It.IsAny<int>())).Returns(book);

            LibraryController controller = new LibraryController(frMock.Object, MessageRepository);
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());

            // Act
            ObjectContent objContent = controller.Assign(1, "SN1").Content as ObjectContent;
            ResponseMessage responseMessage = objContent.Value as ResponseMessage;

            // Assert
            Assert.AreEqual(true, responseMessage.Success);
            Assert.IsNull(responseMessage.Message);
        }

        [TestMethod]
        public void ExtendReturnDateValidationFailTest()
        {
            // Arrange
            IList<Book> entities = null;
            Mock<IRepository<Book>> frMock = new Mock<IRepository<Book>>();
            frMock.Setup(fr => fr.Get()).Returns(entities);

            LibraryController controller = new LibraryController(frMock.Object, MessageRepository);
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());

            // Act
            ObjectContent objContent = controller.ExtendReturnDate(0, 0).Content as ObjectContent;
            ResponseMessage responseMessage = objContent.Value as ResponseMessage;

            // Assert
            Assert.AreEqual(false, responseMessage.Success);
        }

        [TestMethod]
        public void ExtendReturnDateFailTest()
        {
            // Arrange
            Book book = new Book() { ID = 1, Name = "Book One", StudentName = "", ReturnDate = "" };
            Mock<IRepository<Book>> frMock = new Mock<IRepository<Book>>();
            frMock.Setup(fr => fr.Find(It.IsAny<int>())).Returns(book);

            LibraryController controller = new LibraryController(frMock.Object, MessageRepository);
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());

            // Act
            ObjectContent objContent = controller.ExtendReturnDate(1, 1).Content as ObjectContent;
            ResponseMessage responseMessage = objContent.Value as ResponseMessage;

            // Assert
            Assert.AreEqual(false, responseMessage.Success);
            Assert.AreEqual("Book is not borrowed.", responseMessage.Message);
        }

        [TestMethod]
        public void ExtendReturnDateTest()
        {
            // Arrange
            Book book = new Book() { ID = 1, Name = "Book One", StudentName = "SN1", ReturnDate = DateTime.Now.ToShortDateString() };
            Mock<IRepository<Book>> frMock = new Mock<IRepository<Book>>();
            frMock.Setup(fr => fr.Find(It.IsAny<int>())).Returns(book);

            LibraryController controller = new LibraryController(frMock.Object, MessageRepository);
            controller.Request = new HttpRequestMessage();
            controller.Request.SetConfiguration(new HttpConfiguration());

            // Act
            ObjectContent objContent = controller.ExtendReturnDate(1, 1).Content as ObjectContent;
            ResponseMessage responseMessage = objContent.Value as ResponseMessage;

            // Assert
            Assert.AreEqual(true, responseMessage.Success);
            Assert.IsNull(responseMessage.Message);
        }

        [TestMethod]
        public void GetOverdueBooksTest()
        {
            // Arrange
            IList<Book> entities = new List<Book>();
            entities.Add(new Book() { ID = 1, Name = "Book One", StudentName = "SN1", ReturnDate = DateTime.Now.AddDays(-1).ToShortDateString() });
            entities.Add(new Book() { ID = 2, Name = "Book Two", StudentName = "SN1", ReturnDate = DateTime.Now.AddDays(-1).ToShortDateString() });
            entities.Add(new Book() { ID = 3, Name = "Book Three", StudentName = "SN1", ReturnDate = DateTime.Now.ToShortDateString() });

            Mock<IRepository<Book>> frMock = new Mock<IRepository<Book>>();
            frMock.Setup(fr => fr.Get()).Returns(entities);

            LibraryController controller = new LibraryController(frMock.Object, MessageRepository);

            // Act
            IList<Book> books = controller.GetOverdueBooks();

            // Assert
            Assert.IsNotNull(books);
            Assert.AreEqual(2, books.Count);
            Assert.AreEqual("Book One", books[0].Name);
            Assert.AreEqual("Book Two", books[1].Name);
        }
    }
}
