using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace XmlSerialization
{
    using System;

    public class Person
    {
        public string FirstName;
        public string MiddleName;
        [XmlAttribute]
        public string LastName;
    }

    class Program
    {
        static void Main(string[] args)
        {
            Person p = new Person
            {
                FirstName = "Bart",
                MiddleName = "A",
                LastName = "Simpson"
            };

            XmlSerializer x = new XmlSerializer(p.GetType());
            x.Serialize(Console.Out, p);

            var outputFile = $"Person.xml";
            using (var fs = new FileStream(outputFile, FileMode.OpenOrCreate))
            {
                x.Serialize(fs, p);
            }
            using (var fs = new FileStream(outputFile, FileMode.OpenOrCreate))
            {
                var deserialize = (Person)x.Deserialize(fs);
            }


            

            Console.WriteLine();
            Console.ReadLine();
        }
    }
}
