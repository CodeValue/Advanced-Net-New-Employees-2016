using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Serialization
{
    [Serializable]
    class MyData
    {
        public int _data1;
        public int _data2; //...
    }

    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<string> { "Hello", "World" };
            using (var ms = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(ms, list);
                list.Add("Third");
                binaryFormatter.Serialize(ms, list);

                ms.Seek(0, SeekOrigin.Begin);
                var lst1 = (List<string>)binaryFormatter.Deserialize(ms);
                var lst2 = (List<string>)binaryFormatter.Deserialize(ms);

            }

            var myData = new MyData() { _data1 = 1, _data2 = 2 };
            SaveData(myData);
            var loadData = LoadData();
        }

        static void SaveData(MyData data)
        {
            IFormatter fm = new BinaryFormatter();
            using (Stream stm = new FileStream(@"c:\mydata.dat",
                FileMode.Create, FileAccess.Write))
            {
                fm.Serialize(stm, data);
            }
        }

        static MyData LoadData()
        {
            IFormatter fm = new BinaryFormatter();
            MyData data = null; // no need to create instance
            using (Stream stm = new FileStream(@"c:\mydata.dat", FileMode.Open,
                FileAccess.Read))
            {
                return (MyData)fm.Deserialize(stm);
            }
        }


    }
}
