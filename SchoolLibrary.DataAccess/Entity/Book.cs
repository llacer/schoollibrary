using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SchoolLibrary.DataAccess.Entity
{
    public class Book : EntityBase
    {
        private string returnDate;

        [XmlAttribute("Name")]
        public string Name { get; set; }
        [XmlAttribute("StudentName")]
        public string StudentName { get; set; }
        [XmlAttribute("ReturnDate")]
        public string ReturnDate
        {
            get
            {
                return returnDate;
            }
            set
            {
                DateTime dt;
                if (DateTime.TryParse(value, out dt))
                    returnDate = dt.ToShortDateString();
            }
        }

        public override string GetRootName()
        {
            return "Books";
        }
    }
}
