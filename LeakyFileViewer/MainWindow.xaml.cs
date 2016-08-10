using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using Path = System.IO.Path;

namespace LeakyFileViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static Dictionary<string, FileHandler> _filesCache = new Dictionary<string, FileHandler>();
        private FileHandler _currFileHandler;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnOpenFile(object sender, RoutedEventArgs e)
        {
            OpenFile(FilePath.Text);
        }
        private void OnBrowseFile(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog().GetValueOrDefault(false))
            {
                FilePath.Text = openFileDialog.FileName;
                OpenFile(openFileDialog.FileName);
            }
        }
        private void OpenFile(string filePath)
        {
            if (File.Exists(filePath) && !_filesCache.ContainsKey(filePath))
            {
                var fileHandler = new FileHandler(filePath);
                fileHandler.ContentChanged += FileHandler_ContentChanged;
                if (_filesCache.Count == 2)
                {
                    var lastKey = _filesCache.Keys.Last();
                    var handler = _filesCache[lastKey];
                    handler.ContentChanged -= FileHandler_ContentChanged;
                    handler.Dispose();
                    _filesCache.Remove(lastKey);
                }
                _filesCache.Add(filePath, fileHandler);
            }

            _currFileHandler = _filesCache[filePath];
            FileContent.Text = _currFileHandler.Content;
        }

        private void FileHandler_ContentChanged(object sender, string newContent)
        {
            if (sender == _currFileHandler)
            {
                Dispatcher.Invoke(() => FileContent.Text = newContent);
            }
        }


    }

    internal class FileHandler : IDisposable
    {
        private readonly string _path;
        private readonly FileSystemWatcher _fileSystemWatcher;
        private bool _disposed;
        public event EventHandler<string> ContentChanged;

        public FileHandler(string path)
        {
            _path = path;
            Content = File.ReadAllText(path);
            var directoryName = Path.GetDirectoryName(path);
            _fileSystemWatcher = new FileSystemWatcher(directoryName, Path.GetFileName(path));
            _fileSystemWatcher.Changed += FileSystemWatcherOnChanged;
            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        private void FileSystemWatcherOnChanged(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            bool shouldTryAgain = true;//we might get the notification while the file is locked
            while (shouldTryAgain)
            {
                try
                {

                    Content = File.ReadAllText(_path);
                    ContentChanged?.Invoke(this, Content);
                    shouldTryAgain = false;
                }
                catch (IOException ex)
                {
                    //swallow exception so we'll try again
                }
            }
        }


        public string Content { get; set; }
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _fileSystemWatcher.Changed -= FileSystemWatcherOnChanged;
            _disposed = true;
        }


    }
}
