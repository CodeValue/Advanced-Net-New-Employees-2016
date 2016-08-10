using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates2
{
    class MessageEventArgs : EventArgs
    {
        public string Message { get; set; }
    }
    class Messenger
    {
        public Action<string> MessageAvailable; //this is unsafe, the invoke is expose to the outside
        public event Action<string> SafeMessageAvailable;// this is safe, the invoke is not expose to the outside

        private Action<string> _messageAvailable;
        public event Action<string> CustomMessageAvailable
        {
            add
            {
                if (value.Target==null)
                {
                    return;
                }
                
                Console.WriteLine("someone subscribed");
                _messageAvailable += value;
            }
            remove
            {
                Console.WriteLine("someone unsubscribed");
                _messageAvailable -= value;
            }
        }


        public event EventHandler<MessageEventArgs> StandardMessageAvailable;
        public void Send(string message)
        {
            MessageAvailable?.Invoke(message);
            SafeMessageAvailable?.Invoke(message);
            _messageAvailable?.Invoke(message);
            StandardMessageAvailable?.Invoke(this, new MessageEventArgs {Message = message});
        }
    }

    class Program
    {
        delegate bool FilterPredicate(int x);
        delegate bool GenericFilterPredicate<T>(T x);
        static void Main(string[] args)
        {
            //DelegateExample();
            //EventsMotivationExample();
            CustomEventExample();
            StandardNetEvents();

            
        }

        private static void StandardNetEvents()
        {
            var messenger = new Messenger();
            messenger.StandardMessageAvailable += MessengerOnStandardMessageAvailable;
            messenger.Send("Hello standard");
        }

        private static void MessengerOnStandardMessageAvailable(object sender, MessageEventArgs messageEventArgs)
        {
            Console.WriteLine($"standard printer, msg {messageEventArgs.Message}");
        }


        private static void CustomEventExample()
        {
            var messenger = new Messenger();
            messenger.CustomMessageAvailable += StaticPrinter;
            messenger.Send("Hello static");
        }

        private static void StaticPrinter(string msg)
        {
            Console.WriteLine($"I'm static printer, message:{msg}");
        }

        private static void EventsMotivationExample()
        {
            var messenger = new Messenger();
            messenger.MessageAvailable += s => Console.WriteLine($"s1 {s}");
            messenger.MessageAvailable += s => Console.WriteLine($"s2 {s}");
            messenger.MessageAvailable?.Invoke("Melicious Message"); //security breach

            messenger.SafeMessageAvailable += s => Console.WriteLine($"s1 {s}");
            messenger.SafeMessageAvailable += s => Console.WriteLine($"s2 {s}");
            //messenger.SafeMessageAvailable?.Invoke("Melicious Message"); //doesnt compile

            messenger.Send("Hello");
        }

        private static void DelegateExample()
        {
            GenericFilterPredicate<int> myFilters = FilterByModulo2;
            myFilters += FilterByModulo3;


            var evenNumbers = Filter(new[] {1, 2, 3, 4, 5, 6}, FilterByModulo2);
            var by3Numbers = Filter(new[] {1, 2, 3, 4, 5, 6}, FilterByModulo3);
            var filtered = Filter(new[] {1, 2, 3, 4, 5, 6}, myFilters);
            var by4Numbers = Filter(new[] {1, 2, 3, 4, 5, 6}, x => x%4 == 0);

            Action<int> act1; // void Action<T1>(T1 arg1) --> T1 is int 
            Action<int, string> act2; // void Action<T1,T2>(T1 arg1,T2 arg2) --> T1 is int T2 is string

            Func<int, int> func1; // R Func<T1,R>(T1 arg1) --> T1 is int R is int
            Func<int, string, int> func2; // R Func<T1,T2,R>(T1 arg1, T2 arg2) --> T1 is int T2 is string R is int

            foreach (var evenNumber in evenNumbers)
            {
                Console.WriteLine(evenNumber);
            }
        }

        private static bool FilterByModulo3(int i)
        {
            return i % 3 == 0;
        }

        private static bool FilterByModulo2(int i)
        {
            return i % 2 == 0;
        }

        static IEnumerable<int> FilterInts(IEnumerable<int> items, FilterPredicate pred)
        {
            foreach (var item in items)
            {
                if (pred(item))
                {
                    yield return item;
                }
            }
        }
        static IEnumerable<TItem> Filter<TItem>(IEnumerable<TItem> items, GenericFilterPredicate<TItem> pred)
        {
            foreach (var item in items)
            {
                if (pred(item))
                {
                    yield return item;
                }
            }
        }
    }
}
