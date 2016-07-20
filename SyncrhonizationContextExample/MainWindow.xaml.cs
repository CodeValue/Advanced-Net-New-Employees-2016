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

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var synchronizationContext = SynchronizationContext.Current;
            ThreadPool.QueueUserWorkItem(_ =>
            {
                synchronizationContext.Send(o =>
                {
                    MyTextBlock.Text = "Starting the heavy operation";
                },null);
                
                Thread.Sleep(5000);

                synchronizationContext.Send(o =>
                {
                    MyTextBlock.Text = "Finished the heavy operation";
                }, null);
            });
        }
    }
}
