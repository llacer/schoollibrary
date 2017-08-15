using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLibrary.Common.Helpers
{
    public class ConfigHelper
    {
        public static string BooksFilePath
        {
            get
            {
                return ConfigurationManager.AppSettings["BooksFilePath"];
            }
        }

        public static string MessagesFilePath
        {
            get
            {
                return ConfigurationManager.AppSettings["MessagesFilePath"];
            }
        }

        public static int ReturnDays
        {
            get
            {
                string returnDays = ConfigurationManager.AppSettings["ReturnDays"];
                return Int32.Parse(returnDays);
            }
        }
    }
}
