using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPL_UnobservedException
{
    class Program
    {
        static void Main(string[] args)
        {
            Trace.WriteLine("App Started");
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
            Task.Run(() =>
            {
                throw new ApplicationException("Boom");
            }).ContinueWith(prevTask =>
            {
                if (prevTask.IsFaulted)
                {
                    Trace.WriteLine($"Task faulted {prevTask.Exception}");
                }
            });
            Console.ReadLine();
        }

        private static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Trace.WriteLine($"Unobserved Task Exception {e.Exception}");
        }
    }
}
