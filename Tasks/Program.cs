using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tasks
{
    class Program
    {
        static void Main(string[] args)
        {
            //CreateTasks();
            //Continuation();
            //var theTask = AsyncAwaitAgain();
            //Console.WriteLine("AsyncAwaitAgain returned");
            //theTask.Wait();

            //var asyncAwaitComplexStrcutre = AsyncAwaitComplexStrcutre(10);
            //asyncAwaitComplexStrcutre.Wait();

            var asyncAwaitComplexStrcutreBehindTheScenes = AsyncAwaitComplexStrcutre_BehindTheScenes(5);
            asyncAwaitComplexStrcutreBehindTheScenes.Wait();
            Console.WriteLine("Press any key to continue ...");
            Console.ReadLine();
        }

        private static async Task AsyncAwaitAgain()
        {
            Console.WriteLine("AsyncAwaitAgain started");

            await Task.Delay(2000);
            await Task.Delay(2000);
            await Task.Delay(2000);
            await Task.Delay(2000);
            Console.WriteLine("Heavy work is done");
        }

        private static Task AsyncAwaitAgain_BehindTheScenes()
        {
            Console.WriteLine("AsyncAwaitAgain started");

            return Task.Delay(2000)
                .ContinueWith(async _ => await Task.Delay(2000))
                .ContinueWith(async _ => await Task.Delay(2000))
                .ContinueWith(async _ => await Task.Delay(2000))
                .ContinueWith(async _ => await Task.Delay(2000))
                .ContinueWith(_ => Console.WriteLine("Heavy work is done"));
        }
        private static async Task AsyncAwaitComplexStrcutre(int iterations)
        {
            Console.WriteLine("AsyncAwaitAgain started");
            for (int i = 0; i < iterations; i++)
            {
                Console.WriteLine($"iteration {i} started on {Thread.CurrentThread.ManagedThreadId}");
                await Task.Delay(2000);
                Console.WriteLine($"iteration {i} finished on {Thread.CurrentThread.ManagedThreadId}");
                if (DateTime.Now.Second==10)
                {
                    continue;
                }
                Console.WriteLine($"iteration {i} completed");
            }
            Console.WriteLine("Heavy work is done");
        }

        private static Task AsyncAwaitComplexStrcutre_BehindTheScenes(int iterations)//kind of
        {
            Console.WriteLine("AsyncAwaitAgain started");
            var currentTask=Task.Run(()=>Thread.Sleep(2000));
            if (iterations>1)
            {
                for (int i = 0; i < iterations-1; i++)
                {
                    Console.WriteLine($"iteration {i} started on {Thread.CurrentThread.ManagedThreadId}");

                    int id = i;
                    currentTask = currentTask.ContinueWith(_ =>
                    {
                        Thread.Sleep(2000);
                        Console.WriteLine($"iteration {id} completed");
                    });
                    Console.WriteLine($"iteration {i} finished on {Thread.CurrentThread.ManagedThreadId}");

                }
            }

            return currentTask.ContinueWith(_ => { Console.WriteLine("Heavy work is done"); });
        }

        private static void Continuation()
        {
            var heavyWorkTask = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine($"Heavy work is done");
                return 10;
            });

            Task<int> continuationTask =
                heavyWorkTask.ContinueWith(prevTask =>
                {
                    if (prevTask.IsFaulted)
                    {
                        Console.WriteLine($"Previous task is faulted, Exception:{prevTask.Exception}");
                        return 0;
                    }
                    if (prevTask.IsCanceled)
                    {

                        Console.WriteLine($"Previous task is cancelled");
                        return -1;
                    }

                    int result = prevTask.Result;
                    Console.WriteLine("Making complicated calculation");
                    return ++result;
                });

            Console.WriteLine($"Final result: {continuationTask.Result}"); //the call to Result blocks the current thread
        }

        private static void CreateTasks()
        {
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 5; i++)
            {
                int id = i;

                tasks.Add(Task.Run(() =>
                {
                    Thread.Sleep(1000);
                    Console.WriteLine($"Hello i'm Task {id} Thread: {Thread.CurrentThread.ManagedThreadId}");
                }));
            }

            Task.WaitAll(tasks.ToArray()); //blocks the current thread
            
        }
    }
}
