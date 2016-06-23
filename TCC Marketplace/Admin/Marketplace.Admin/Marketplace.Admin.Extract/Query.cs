using System.Collections.Generic;
using System.Xml.Serialization;

namespace Marketplace.Admin.Extract
{
    public class Query
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlElementAttribute("Sql")]
        public string Sql { get; set; }

        [XmlElement("FileName")]
        public string FileName { get; set; }
    }
    public class DataSource
    {
        public List<Query> Queries
        {
            get;
            set;
        }
    }
}