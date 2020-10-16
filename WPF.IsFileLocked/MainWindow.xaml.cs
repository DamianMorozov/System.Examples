using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace WPF.IsFileLocked
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly FileAccessLocker _fileAccessLocker = FileAccessLocker.Instance;

        #region Window

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            fieldFilePath.Text = string.Empty;
            fieldResult.Text = string.Empty;
        }

        #endregion

        #region User actions

        private void ButtonSetFilePath_Click(object sender, RoutedEventArgs e)
        {
            using (var openFileDialog = new System.Windows.Forms.OpenFileDialog() { InitialDirectory = Environment.CurrentDirectory })
            {
                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    fieldFilePath.Text = openFileDialog.FileName;
                    fieldResult.Text = string.Empty;
                }
            }
        }

        private void ButtonCheck_Click(object sender, RoutedEventArgs e)
        {
            fieldResult.Clear();

            if (File.Exists(fieldFilePath.Text))
            {
                var stopWatch = Stopwatch.StartNew();
                var strFileIsFree = @"File is free.";
                var strFileIsBusy = @"File is busy!";
                Exception exception;
                switch (fieldAlgorithm.SelectedIndex)
                {
                    // Use FileStream
                    case 1:
                        fieldResult.Text += IsFileLockedUseFileStream(fieldFilePath.Text, out exception)
                            ? strFileIsBusy + Environment.NewLine + exception
                            : strFileIsFree;
                        break;
                    // Use FileSystemWatcher
                    case 2:
                        fieldResult.Text += IsFileLockedUseFileSystemWatcher(fieldFilePath.Text, out exception)
                            ? strFileIsBusy + Environment.NewLine + exception
                            : strFileIsFree;
                        break;
                    // Use Use FindProcessLock
                    case 3:
                        string message = null;
                        var fileProcessLock = _fileAccessLocker.IsProcessLock(fieldFilePath.Text, out message);
                        fieldResult.Text += fileProcessLock
                            ? strFileIsBusy + Environment.NewLine + message
                            : strFileIsFree;
                        break;
                    // Use FileInfo
                    default:
                        fieldResult.Text += IsFileLockedUseFileInfo(fieldFilePath.Text, out exception)
                            ? strFileIsBusy + Environment.NewLine + exception
                            : strFileIsFree;
                        break;
                }
                stopWatch.Stop();
                fieldResult.Text += Environment.NewLine + @"Time elapsed: " + stopWatch.Elapsed.ToString();
            }
            else
            {
                fieldResult.Text = @"Input valid full file name!";
            }
        }

        #endregion

        #region Use FileInfo

        private bool IsFileLockedUseFileInfo(string fileName, out Exception exception)
        {
            exception = null;

            if (string.IsNullOrEmpty(fileName))
                return false;

            FileInfo fileInfo = new FileInfo(fileName);
            FileStream fileStream = null;
            try
            {
                fileStream = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException exc)
            {
                exception = exc;
                return true;
            }
            finally
            {
                fileStream?.Close();
            }

            return false;
        }

        #endregion

        #region Use FileStream

        private bool IsFileLockedUseFileStream(string fileName, out Exception exception)
        {
            exception = null;

            if (string.IsNullOrEmpty(fileName))
                return false;

            try
            {
                using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    //
                }
            }
            catch (IOException exc)
            {
                exception = exc;
                return true;
            }

            return false;
        }

        #endregion

        #region Use FileSystemWatcher

        private bool IsFileLockedUseFileSystemWatcher(string fileName, out Exception exception)
        {
            exception = null;

            if (string.IsNullOrEmpty(fileName))
                return false;

            try
            {
                var fileSystemWatcher = new FileSystemWatcher
                {
                    Path = Path.GetDirectoryName(fileName),
                    NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                    Filter = Path.GetFileName(fileName)
                };

                // Add event handlers
                fileSystemWatcher.Changed += new FileSystemEventHandler(OnChanged);
                fileSystemWatcher.Created += new FileSystemEventHandler(OnChanged);
                fileSystemWatcher.Deleted += new FileSystemEventHandler(OnChanged);
                fileSystemWatcher.Renamed += new RenamedEventHandler(OnRenamed);

                // Begin watching
                fileSystemWatcher.EnableRaisingEvents = true;
            }
            catch (IOException exc)
            {
                exception = exc;
                return true;
            }

            return false;
        }

        private void ProcessInfoAddInOtherThread(string msg)
        {
            Application.Current?.Dispatcher?.BeginInvoke(new Action(() =>
            {
                fieldResult.Text += msg;
            }));
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            ProcessInfoAddInOtherThread(@"File: " + e.FullPath + @" " + e.ChangeType);
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            ProcessInfoAddInOtherThread($@"File: {e.OldFullPath} renamed to {e.FullPath}");
        }

        #endregion
    }
}
