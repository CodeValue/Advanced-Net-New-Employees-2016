using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TPL_Continuation
{
    public static class TaskExtensions
    {
        public static Task<T> LogError<T>(this Task<T> src)
        {
            src.ContinueWith(_ =>
            {
                Debug.WriteLine($"task faulted {src.Exception}");
            }, TaskContinuationOptions.OnlyOnFaulted);
            return src;
        }
        public static Task LogError(this Task src)
        {
            src.ContinueWith(_ =>
            {
                Debug.WriteLine($"task faulted {src.Exception}");
            }, TaskContinuationOptions.OnlyOnFaulted);
            return src;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //ContinueWithExample();
            //ContinueWithMany();
            AggregateExceptionExample();
            Console.ReadLine();
        }

        private static void AggregateExceptionExample()
        {
            var t1=Task.Run(() => { throw new Exception("T1 exception"); });
            var t2=Task.Run(() => { throw new Exception("T2 exception"); });

            Task.WhenAll(t1, t2)
                .ContinueWith(prev =>
                {
                    Console.WriteLine($"{prev.Exception} {prev.Exception.InnerExceptions[0]}");
                });
        }

        private static void ContinueWithMany()
        {
            Task delay2sec = Task.Delay(2000);
            Task delay3sec = Task.Delay(3000);

            Task.WhenAll(delay3sec, delay2sec)
                .ContinueWith(_ =>
                {
                    Console.WriteLine("Done");
                });

            Task.WhenAny(delay3sec, delay2sec)
                .ContinueWith(prevTask =>
                {
                    if (prevTask == delay2sec)
                    {
                        Console.WriteLine("Done 2sec");
                    }
                    else
                    {
                        Console.WriteLine("Done 3sec");

                    }
                });
        }

        private static void ContinueWithExample()
        {
            var originalTask = Task.Run(() =>
            {
                Thread.Sleep(2000);
                Console.WriteLine("Task1 completed");
                throw new Exception("t1 has died");
            })
            .LogError();

            originalTask.ContinueWith(_ => { Console.WriteLine("Im running as well"); });

            var finalTask =
                originalTask.ContinueWith(prevTask =>
                {
                    if (prevTask.IsFaulted)
                    {
                        Console.WriteLine($"t1 was faulted - {prevTask.Exception} ");
                    }
                    else
                    {
                        Console.WriteLine("t1 was ok");
                    }

                    return prevTask.IsFaulted;
                }).ContinueWith(prevTask => { Console.WriteLine(prevTask.Result); });

            finalTask.Wait();
            Console.WriteLine($"final task status is {finalTask.Status}");
        }
    }
}
