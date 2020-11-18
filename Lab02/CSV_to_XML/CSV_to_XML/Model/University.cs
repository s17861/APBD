using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace CSV_to_XML.Model
{
    [Serializable]
    public class ActiveStudies
    {
        [XmlAttribute(attributeName:"name")]
        public string name { get; set; }
        [XmlAttribute(attributeName: "numberOfStudents")]
        public int studentCount { get; set; }
    }
    [Serializable]
    public class University
    {
        [JsonPropertyName("createdAt")]
        [XmlAttribute(attributeName: "createdAt")]
        public DateTime createdAt { get; }
        [JsonPropertyName("author")]
        [XmlAttribute(attributeName: "author")]
        public string author { get; }
        [JsonPropertyName("studenci")]
        public HashSet<Student> students { get; }
        public HashSet<ActiveStudies> activeStudies { get; }

        public University()
        {
            createdAt = DateTime.Now;
            author = "Adam Chyciński";
            students = new HashSet<Student>();
            activeStudies = new HashSet<ActiveStudies>();
        }

        public void AddStudent(Student s, string study)
        {
            students.Add(s);
            var temp = activeStudies.FirstOrDefault(s => s.name == study);
            if(temp == null)
            {
                temp = new ActiveStudies
                {
                    name = study,
                    studentCount = 0
                };
                activeStudies.Add(temp);
            }
            temp.studentCount += 1;
        }
    }
}
