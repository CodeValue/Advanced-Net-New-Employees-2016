using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPoolExamples
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Main thread is {Thread.CurrentThread.ManagedThreadId}");
            while (true)
            {
                Console.WriteLine("Enter file name");
                var filename = Console.ReadLine();
                //ThreadPool.QueueUserWorkItem(ProcessFileInBackground, filename);
                ThreadPool.QueueUserWorkItem(_ => { ProcessFile(filename); });
            }
            



            Thread.Sleep(1000);
            ThreadPool.QueueUserWorkItem((o) =>
            {
                Console.WriteLine($"Thread is running {Thread.CurrentThread.ManagedThreadId}");
            });
            Thread.Sleep(1000);

            ThreadPool.QueueUserWorkItem((o) =>
            {
                Console.WriteLine($"Thread is running {Thread.CurrentThread.ManagedThreadId}");
            });
            ThreadPool.QueueUserWorkItem((o) =>
            {
                Console.WriteLine($"Thread is running {Thread.CurrentThread.ManagedThreadId}");
            });

            Console.ReadLine();
        }

        private static void ProcessFileInBackground(object state)
        {
            var filename = state.ToString();
            ProcessFile(filename);
        }

        private static void ProcessFile(string filename)
        {
            Console.WriteLine($"Thread is running {Thread.CurrentThread.ManagedThreadId} on file {filename}");
        }
    }
}
