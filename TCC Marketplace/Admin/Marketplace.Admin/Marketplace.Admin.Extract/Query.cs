using System.Collections.Generic;
using System.Xml.Serialization;

namespace Marketplace.Admin.Extract
{
    /// <summary>
    /// Query deserializer class.
    /// </summary>
    public class Query
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlElementAttribute("Sql")]
        public string Sql { get; set; }

        [XmlElement("FileName")]
        public string FileName { get; set; }
    }

    /// <summary>
    /// Query deserializer class.
    /// </summary>
    public class DataSource
    {
        public List<Query> Queries
        {
            get;
            set;
        }
    }
}