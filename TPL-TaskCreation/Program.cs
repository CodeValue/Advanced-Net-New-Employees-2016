using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPL_TaskCreation
{
    class Program
    {
        static Task<int> CalcualteTheAmontOfParticalesInUniverse()
        {
            return Task.FromResult(int.MaxValue);
        }
        static Task DoSomethingInBackground()
        {
            return Task.CompletedTask;
        }

        static Task FaultyMethod(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return Task.FromException(new ApplicationException("Boom"));
            }

            return Task.Run(() => { });
        }
        static void Main(string[] args)
        {
            Console.WriteLine($"The amount of particales {CalcualteTheAmontOfParticalesInUniverse().Result}");
            Console.WriteLine($"Faulty Method");
            FaultyMethod("tamir").ContinueWith(prevTask =>
            {
                if (prevTask.IsFaulted)
                {
                    //Do something
                }
            });
            Console.WriteLine("After Faulty Method");

            TaskCompletionSource<int> tcs=new TaskCompletionSource<int>();
            Task<int> task = tcs.Task;
            task.ContinueWith(t => Console.WriteLine($"task completed, Result={t.Result}"));

            //...
            tcs.SetResult(10);
            bool success = tcs.TrySetResult(10);
            //tcs.SetException(new ApplicationException(""));

            Console.ReadLine();
        }
    }
}
