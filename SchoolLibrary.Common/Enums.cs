using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLibrary.Common
{
    public enum MessagesEnum : int
    {
        BOOK_NOT_FOUND = 1,
        BOOK_ALREADY_BORROWED = 2,
        BOOK_NOT_BORROWED = 3,
        INVALID_RETURN_DATE = 4,
        INVALID_INPUT_PARAMETERS = 5,
        APPLICATION_EXCEPTION = 6
    }
}
