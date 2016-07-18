using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication14
{
    class Program
    {
        private static MyCallback _callback;

        delegate string MyCallback(int x, string y);
        static void Main(string[] args)
        {
            var program = new Program();
            MyCallback callback= MyMethod;
            callback += program.MyPrinter;
            var combined=Delegate.Combine(callback, new MyCallback(program.MyPrinter));
            callback.Invoke(5,"hello");
            callback(5, "hello");

            Delegate[] invocationList = callback.GetInvocationList();
            foreach (MyCallback littleCallback in invocationList)
            {
                try
                {
                    callback(5, "hello");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("There was an error");
                }
            }
            _callback = callback;
            IAsyncResult ar=callback.BeginInvoke(5, "hello", CallbackEnded, "tamir");
            
        }

        private static void CallbackEnded(IAsyncResult ar)
        {
            var sender = ar.AsyncState.ToString();
            string result = _callback.EndInvoke(ar);
        }

        string MyPrinter(int x, string str)
        {
            Console.WriteLine($"x={x} str={str}");
            return str;

        }
        static string MyMethod(int x, string str)
        {
            Console.WriteLine($"x={x} str={str}");
            return str;
        }
    }
}
