using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SchoolLibrary.DataAccess.Entity
{
    public class Message : EntityBase
    {
        [XmlAttribute("Text")]
        public string Text { get; set; }

        public override string GetRootName()
        {
            return "Messages";
        }
    }
}
