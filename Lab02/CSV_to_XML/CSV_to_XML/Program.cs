using CSV_to_XML.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

namespace CSV_to_XML
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length != 3)
                {
                    Console.WriteLine(String.Format("Usage: {0} input.csv output.xml xml", AppDomain.CurrentDomain.FriendlyName));
                    throw new ArgumentException("Incorrect number of arguments");
                }

                var uni = new University();
                foreach (var line in File.ReadAllLines(args[0]))
                {
                    var split = line.Split(",");
                    if (split.Length != 9)
                    {
                        return;
                        // TODO logging, quit
                    }
                    var studyName = split[2];
                    uni.AddStudent(new Student
                    {
                        id = $"s{split[4]}",
                        firstName = split[0],
                        lastName = split[1],
                        dob = DateTime.Parse(split[5]).ToShortDateString(),
                        email = split[6],
                        mothersName = split[7],
                        fathersName = split[8],
                        studies = new Study
                        {
                            name = studyName,
                            mode = split[3]
                        }
                    }, studyName);
                }

                using (StreamWriter sw = new StreamWriter(args[1]))
                {
                    switch (args[2])
                    {
                        case "xml":
                            XmlSerializer serializer = new XmlSerializer(typeof(University), new XmlRootAttribute("uczelnia"));
                            serializer.Serialize(sw, uni);
                            break;
                        case "json":
                            var json = JsonSerializer.Serialize(uni, new JsonSerializerOptions{WriteIndented = true});
                            sw.Write(json);
                            break;
                        default:
                            throw new ArgumentException("unknown output type", "args[2]");
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
