using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SyncrhonizationContextExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            //DoWork();
            await DoWorkAsync();
        }

        private void DoWork()
        {
            var synchronizationContext = SynchronizationContext.Current;
            ThreadPool.QueueUserWorkItem(_ =>
            {
                synchronizationContext.Send(o => { MyTextBlock.Text = "Starting the heavy operation"; }, null);

                Thread.Sleep(5000);

                synchronizationContext.Send(o => { MyTextBlock.Text = "Finished the heavy operation"; }, null);
            });
        }

        private async Task DoWorkAsync()
        {
            var schoolLogic = new SchoolLogic();
            Button.IsEnabled = false;
            MyTextBlock.Text = "Starting the heavy operation";
            //var report = await schoolLogic.GenerateReportAsync();
            var report = await Task.Run(()=>schoolLogic.GenerateReport());
            MyTextBlock.Text = $"Finished the heavy operation, report has {report.Count()} records";
            Button.IsEnabled = true;
        }
    }

    class SchoolLogic
    {
        public IEnumerable<Grade> GenerateReport()
        {
            Thread.Sleep(5000);
            return Enumerable.Empty<Grade>();
        }
        public async Task<IEnumerable<Grade>> GenerateReportAsync()
        {
            var managedThreadId1 = Thread.CurrentThread.ManagedThreadId;
            var report = new List<Grade>();
            IEnumerable<Course> courses = await GetStudentCoursesAsync().ConfigureAwait(false);
            var managedThreadId2 = Thread.CurrentThread.ManagedThreadId;

            foreach (var course in courses)
            {
                IEnumerable<Grade> grades;
                try
                {
                    grades = await GetGradesAsync(course.Name).ConfigureAwait(false);
                }
                catch (Exception)
                {
                    grades = Enumerable.Empty<Grade>();
                }
                var avg = grades.Select(g => g.TheGrade).Average();
                report.Add(new Grade(avg));
            }
            return report;
        }

        private Task<IEnumerable<Grade>> GetGradesAsync(string name)
        {
            return Task.FromResult(new[] { new Grade(100) }.AsEnumerable());
        }

        private Task<IEnumerable<Course>> GetStudentCoursesAsync()
        {
            return Task.Run(() => { return new[] { new Course { Name = "Infi1" } }.AsEnumerable(); });
        }
    }

    internal class Course
    {
        public string Name { get; set; }
    }

    internal class Grade
    {
        public double TheGrade { get; set; }

        public Grade(double theGrade)
        {
            TheGrade = theGrade;
        }
    }
}
