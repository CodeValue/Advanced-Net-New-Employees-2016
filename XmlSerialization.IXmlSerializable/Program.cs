using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace XmlSerialization.IXmlSerializable
{
    public class Person : System.Xml.Serialization.IXmlSerializable
    {

        // Private state

        private string personName;


        // Constructors

        public Person(string name)
        {
            personName = name;
        }

        public Person()
        {
            personName = null;
        }


        // Xml Serialization Infrastructure

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteString(personName);
        }

        public void ReadXml(XmlReader reader)
        {
            personName = reader.ReadString();
        }

        public XmlSchema GetSchema()
        {
            return (null);
        }


        // Print

        public override string ToString()
        {
            return (personName);
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            var person = new Person("Bart");
            XmlSerializer x = new XmlSerializer(typeof(Person));
            x.Serialize(Console.Out,person);
        }
    }
}
