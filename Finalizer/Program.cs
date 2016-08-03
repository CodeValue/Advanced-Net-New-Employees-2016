using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finalizer
{
    class HolderOfNativeResource
    {
        private ResourceHolderWrapper _innerResourceHolderWrapper;

        public HolderOfNativeResource()
        {
            _innerResourceHolderWrapper = new ResourceHolderWrapper(this);
            throw new ApplicationException("Boom");
        }

        ~HolderOfNativeResource()
        {
            Console.WriteLine("A Finalizer");
        }
    }

    class ResourceHolderWrapper
    {
        private readonly HolderOfNativeResource _innerHolderOfNativeResource;

        public ResourceHolderWrapper(HolderOfNativeResource innerHolderOfNativeResource)
        {
            _innerHolderOfNativeResource = innerHolderOfNativeResource;
            Console.WriteLine("B constructed");
        }

        ~ResourceHolderWrapper()
        {
            Console.WriteLine("B finalizer");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            WeirdCase();

            Console.ReadLine();
        }

        private static void WeirdCase()
        {
            try
            {
                var a = new HolderOfNativeResource();
            }
            catch (Exception)
            {

                Console.WriteLine("A can't be constructed");
            }
        }
    }
}
