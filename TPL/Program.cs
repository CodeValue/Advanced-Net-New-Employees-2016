using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TPL
{
    class Program
    {
        static void Main(string[] args)
        {
            Task task = new Task(() => { Console.WriteLine("The empty task is running..."); });
            Task<int> typedTask = new Task<int>(() =>
            {
                Thread.Sleep(3000);
                return 5;
            });
            task.Start();
            task.Wait();

            Console.WriteLine($"The task state is {typedTask.Status}");
            typedTask.Start();
            Console.WriteLine($"Waiting for the task {typedTask.Status}");
            Console.WriteLine($"The task result is {typedTask.Result}");
            Console.WriteLine($"Teh task state is {typedTask.Status}");

            Console.WriteLine("Done waiting for the task");
            
            Console.ReadLine();
        }
    }
}
