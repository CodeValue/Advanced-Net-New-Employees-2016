using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cancellation
{
    class Program
    {
        static void Main(string[] args)
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;
            cancellationTokenSource.Cancel();
            var t2 = Task.Run(() =>
              {
                  while (true)
                  {
                      cancellationToken.ThrowIfCancellationRequested();
                      Console.WriteLine("In loop");
                      Thread.Sleep(1000);
                  }
              }, cancellationToken)
              .ContinueWith(prev =>
              {
                  Console.WriteLine($"t2 status is now {prev.Status}");
              });

            
            cancellationToken.Register(() => { Console.WriteLine("this will run when cancelled"); });

            Thread.Sleep(4000);
            cancellationTokenSource.Cancel();
            Console.ReadLine();
        }
    }
}
