using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsAndReferences
{
    class Publihser
    {
        readonly List<WeakReference<EventHandler<string>>> _consumers = new List<WeakReference<EventHandler<string>>>();
        public event EventHandler<string> SomethingHappened
        {
            add { _consumers.Add(new WeakReference<EventHandler<string>>(value)); }
            remove
            {
                var registeredClientWeakReference = _consumers.First(wr =>
                  {
                      EventHandler<string> client;
                      if (wr.TryGetTarget(out client))
                      {
                          return client == value;
                      }
                      return false;
                  });

                _consumers.Remove(registeredClientWeakReference);
            }
        }

        public void ReportEvent(string msg)
        {
            List<WeakReference<EventHandler<string>>> itemsToRemove = new List<WeakReference<EventHandler<string>>>();
            foreach (WeakReference<EventHandler<string>> consumer in _consumers)
            {
                EventHandler<string> client;
                if (consumer.TryGetTarget(out client))
                {
                    client(this, msg);
                }
                else
                {
                    itemsToRemove.Add(consumer);
                }

            }

            foreach (var weakReference in itemsToRemove)
            {
                _consumers.Remove(weakReference);
            }
        }
    }

    class Consumer
    {
        public void Print(object sender, string message)
        {
            Console.WriteLine("Consumer "+message);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string data = "some data";
            var publihser = new Publihser();
            publihser.SomethingHappened += (s, m) => { Console.WriteLine($"consumer1: {m} Data:{data}"); };
            publihser.SomethingHappened += (s, m) => { Console.WriteLine("consumer2: " + m); };
            Register(publihser);
            publihser.ReportEvent("Winter is coming");

            GC.Collect();
            publihser.ReportEvent("another message");

            Console.ReadLine();
        }

        private static void Register(Publihser publihser)
        {
            var consumer = new Consumer();
            publihser.SomethingHappened += consumer.Print;
        }

        private static void ReportMethod(object sender, string e)
        {
            Console.WriteLine("Report method");
        }
    }
}
