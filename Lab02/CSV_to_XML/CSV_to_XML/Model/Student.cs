using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace CSV_to_XML.Model
{
    [Serializable]
    public class Student
    {
        [XmlAttribute(attributeName: "indexNumber")]
        [JsonPropertyName("indexNumber")]
        public string id { get; set; }
        public string email { get; set; }
        [XmlElement(elementName: "fname")]
        [JsonPropertyName("fname")]
        public string firstName { get; set; }
        [XmlElement(elementName: "lname")]
        [JsonPropertyName("lname")]
        public string lastName { get; set; }
        [XmlElement(elementName: "birthdate")]
        [JsonPropertyName("birthdate")]
        public string dob { get; set; }
        public string mothersName { get; set; }
        public string fathersName { get; set; }
        public Study studies { get; set; }

    }
}
