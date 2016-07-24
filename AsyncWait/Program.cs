using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncWait
{
    class Program
    {
        static void Main(string[] args)
        {
            //ReadHtmlWithTasks();
            ReadHtmlWithAsyncAwait().Wait();
            Console.WriteLine("After the call to the async method");
            //Console.ReadLine();
        }

        private static async Task ReadHtmlWithAsyncAwait()
        {
            var httpClient = new HttpClient();
            try
            {
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                HttpResponseMessage message = await httpClient.GetAsync("http://codevalue.net");
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

                Console.WriteLine($"response code is {message.StatusCode}");
                var html = await message.Content.ReadAsStringAsync();
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

                using (var file = File.Open(@"c:\codevalue.html",FileMode.OpenOrCreate))
                using (var writer=new StreamWriter(file))
                {
                    await writer.WriteAsync(html);
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

                }
                Console.WriteLine(html);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
           
        }

        private static void ReadHtmlWithTasks()
        {
            var httpClient = new HttpClient();
            Task<HttpResponseMessage> getHtml = httpClient.GetAsync("http://codevalue.net");
            getHtml.ContinueWith(prevTask =>
            {
                var htmlStringAsync = prevTask.Result.Content.ReadAsStringAsync();
                htmlStringAsync.ContinueWith(prevTask2 =>
                {
                    string html = prevTask2.Result;
                    Console.WriteLine(html);
                });
            });
        }
    }
}
