using SchoolLibrary.Common;
using SchoolLibrary.Common.Helpers;
using SchoolLibrary.DataAccess.Entity;
using SchoolLibrary.DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Http;

namespace SchoolLibrary.Services.Controllers
{
    public class LibraryController : ApiController
    {
        private IRepository<Book> BookRepository { get; set; }
        private IRepository<Message> MessageRepository { get; set; }

        public LibraryController(IRepository<Book> bookRepository, IRepository<Message> messageRepository)
        {
            BookRepository = bookRepository;
            MessageRepository = messageRepository;
        }

        [HttpGet]
        public IList<Book> Get()
        {
            IList<Book> books = null;
            try
            {
                books = BookRepository.Get().ToList();
            }
            catch (Exception)
            {
                //Log Exception
            }

            return books;
        }

        [HttpPost]
        public HttpResponseMessage Assign(int id, string student)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                if (id <= 0 || string.IsNullOrEmpty(student))
                    responseMessage.Message = GetErrorMessage(MessagesEnum.INVALID_INPUT_PARAMETERS);
                else
                {
                    Book book = BookRepository.Find(id);
                    if (book != null)
                    {
                        if (!string.IsNullOrEmpty(book.StudentName))
                        {
                            responseMessage.Success = false;
                            responseMessage.Message = GetErrorMessage(MessagesEnum.BOOK_ALREADY_BORROWED);
                        }
                        else
                        {
                            book.ReturnDate = DateTime.Now.Date.AddDays(ConfigHelper.ReturnDays).ToString();
                            book.StudentName = student;
                            BookRepository.Update(book);
                            responseMessage.Success = true;
                        }
                    }
                    else
                    {
                        responseMessage.Success = false;
                        responseMessage.Message = GetErrorMessage(MessagesEnum.BOOK_NOT_FOUND);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, responseMessage);
            }
            catch (Exception)
            {
                //Log Exception
                responseMessage.Message = GetErrorMessage(MessagesEnum.APPLICATION_EXCEPTION);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, responseMessage);
            }
        }

        [HttpPost]
        public HttpResponseMessage ExtendReturnDate(int id, int days)
        {
            ResponseMessage responseMessage = new ResponseMessage();
            try
            {
                if (id <= 0 || days <= 0)
                    responseMessage.Message = GetErrorMessage(MessagesEnum.INVALID_INPUT_PARAMETERS);
                else
                {
                    Book book = BookRepository.Find(id);
                    if (book != null)
                    {
                        if (string.IsNullOrEmpty(book.StudentName))
                        {
                            responseMessage.Success = false;
                            responseMessage.Message = GetErrorMessage(MessagesEnum.BOOK_NOT_BORROWED);
                        }
                        else
                        {
                            DateTime returnDate;
                            if (!string.IsNullOrEmpty(book.ReturnDate) && DateTime.TryParse(book.ReturnDate, out returnDate))
                            {
                                book.ReturnDate = returnDate.AddDays(days).ToShortDateString();
                                BookRepository.Update(book);
                                responseMessage.Success = true;
                            }
                            else
                            {
                                responseMessage.Success = false;
                                responseMessage.Message = GetErrorMessage(MessagesEnum.INVALID_RETURN_DATE);
                            }
                        }
                    }
                    else
                    {
                        responseMessage.Success = false;
                        responseMessage.Message = GetErrorMessage(MessagesEnum.BOOK_NOT_FOUND);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, responseMessage);
            }
            catch (Exception)
            {
                //Log Exception
                responseMessage.Message = GetErrorMessage(MessagesEnum.APPLICATION_EXCEPTION);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, responseMessage);
            }
        }

        [Route("api/library/getoverduebooks")]
        [HttpGet]
        public IList<Book> GetOverdueBooks()
        {
            IList<Book> books = null;
            try
            {
                books = BookRepository.Get().Where(b => !string.IsNullOrEmpty(b.ReturnDate) && DateTime.Parse(b.ReturnDate) < DateTime.Now.Date).ToList();
            }
            catch (Exception)
            {
                //Log Exception
            }
            return books;
        }

        private string GetErrorMessage(MessagesEnum id)
        {
            Message message = MessageRepository.Find((int)id);
            if (message != null)
                return message.Text;
            else
                return string.Empty;
        }
    }
}
