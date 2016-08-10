using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkSchool.Model
{
    public class SchoolContext : DbContext
    {

        public SchoolContext() : base("SchoolContext")
        {
            Database.SetInitializer(new SchoolInitializer());
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .Property(c => c.CourseID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            modelBuilder.Entity<Department>().Property(t => t.Name).HasMaxLength(50);
            modelBuilder.Entity<Department>().Property(t => t.Name).IsRequired();
            modelBuilder.Entity<Department>().Ignore(t => t.Budget);

        }
    }
}
