using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Threading
{
    class LockableClass
    {
        object _locker=new object();
        public void Print()
        {
            Monitor.Enter(this);
            Console.WriteLine("Success");
            Monitor.Exit(this);

        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var lockableClass = new LockableClass();
            Monitor.Enter(lockableClass);

            //...

            var t=new Thread(o=>lockableClass.Print());//this will block my app
            t.Start();
            Console.WriteLine("Out");
            Console.ReadLine();
        }
    }
}
