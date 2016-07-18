using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqExamples
{
    public static class MyExtensions
    {
        public static IEnumerable<T> MyWhere<T>(this IEnumerable<T> coll,
            Func<T, bool> pred)
        {
            foreach (var item in coll)
            {
                if (pred(item))
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<R> MySelect<T, R>(this IEnumerable<T> coll,
            Func<T, R> func)
        {
            foreach (var item in coll)
            {
                yield return func(item);
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var ints = new[] { 1, 2, 3, 4, 5 };
            Func<int, bool> pred = x => x % 2 == 0;
            var result = ints
                .MyWhere(x => x % 2==0)
                .MySelect(x=>x*2)
                .ToList();

        }
    }
}
