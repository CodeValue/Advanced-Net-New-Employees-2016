using System.Collections.Generic;

namespace EntityFrameworkSchool.Model
{
    public class Course
    {
        public int CourseID { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }

        public virtual ICollection<Enrollment> Enrollments
        {
            get;
            set;
        }
    }
}