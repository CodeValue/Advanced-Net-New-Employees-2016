using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFrameworkSchool.Model;

namespace EntityFrameworkSchool
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new SchoolContext())
            {
                foreach (var course in ctx.Courses)
                {
                    Console.WriteLine($"{course.CourseID} - {course.Title}");
                }
            }
        }
    }
}
