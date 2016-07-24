using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadStatic
{
    class MyClass
    {
        [ThreadStatic]
        static int _accumlator;

        public string Name { get; set; }
        public void Add(int x)
        {
            _accumlator += x;
        }

        public void Print()
        {
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} Name:{Name} Value:{_accumlator}");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //ThreadStatic();
            //WithoutThreadStatic();
            WithThreadLocal();
            Console.ReadLine();
        }

        private static void WithThreadLocal()
        {
            var rnd = new ThreadLocal<Random>(() =>
                new Random(Guid.NewGuid().GetHashCode()));
            var numbers = Enumerable.Range(0, 1000000)
                .AsParallel().
                Select(n => rnd.Value.Next(100));
            Console.WriteLine(numbers.Average()); // result: ~50

        }

        private static void WithoutThreadStatic()
        {
            var rnd = new Random();
            var numbers = Enumerable.Range(0, 1000000)
                .AsParallel()
                .Select(n => rnd.Next(100));
            Console.WriteLine(numbers.Average());
        }

        private static void ThreadStatic()
        {
            var myClass = new MyClass() { Name = "Test1" };
            var t1 = Task.Run(() =>
            {
                for (int i = 0; i < 10; i++)
                {
                    myClass.Add(i);
                    myClass.Print();
                    Thread.Sleep(1000);
                }
            });

            var t2 = Task.Run(() =>
            {
                for (int i = 20; i < 100; i += 20)
                {
                    myClass.Add(i);
                    myClass.Print();
                    Thread.Sleep(1000);
                }
            });

            t1.Wait();
            t2.Wait();
            myClass.Print();
        }
    }
}
