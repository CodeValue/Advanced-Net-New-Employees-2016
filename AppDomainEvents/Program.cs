using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TestClassLibrary;

namespace AppDomainEvents
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve+=CurrentDomainOnAssemblyResolve;
            Console.WriteLine("Before exit");
            Foo();
            Console.ReadLine();
        }

        private static void Foo()
        {
            var myType = new MyType();
            Console.WriteLine("MyType Created");
        }

        private static Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            Console.WriteLine($"AssemblyName:{args.Name} Requesting:{args.RequestingAssembly}");
            return Assembly.LoadFrom(@"C:\Users\Tamir\Source\Repos\Advanced-Net-New-Employees-2016\AppDomainEvents\bin\TestClassLibrary.dll");
        }
    }
}
