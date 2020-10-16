using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Wpf.ProcessSpy
{
    // ReSharper disable once UnusedMember.Global
    public partial class MainWindow
    {
        #region Public and private fields and properties

        private Task _task;
        private readonly int _timeout = 1_000;
        private bool _isActive;

        #endregion

        #region Constructor and destructor

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            ButtonStartTask_OnClick(sender, null);
        }

        private void MainWindow_OnClosed(object sender, EventArgs e)
        {
            ButtonStopTask_OnClick(sender, null);
        }

        #endregion

        #region Public and private methods

        private void ButtonStartTask_OnClick(object sender, RoutedEventArgs e)
        {
            if (_task is null)
            {
                _task = Task.Run(async () =>
                {
                    _isActive = true;
                    while (_isActive)
                    {
                        GetProcess();
                        await Task.Delay(TimeSpan.FromMilliseconds(_timeout));
                    }
                });
            }
        }

        private void ButtonStopTask_OnClick(object sender, RoutedEventArgs e)
        {
            Task.Run(async () =>
            {
                _isActive = false;
                while (!(_task is null) && !_task.IsCompleted)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(_timeout));
                }
                _task?.Dispose();
                _task = null;
                var dt = DateTime.Now;
                WPF.Utils.InvokeTextBox.SetText(fieldOut, $"{dt.Hour}:{dt.Minute}:{dt.Second}  The task is stopped.");
            });
        }

        private void GetProcess()
        {
            try
            {
                if (NativeMethods.GetCursorPos(out var point))
                {
                    var hWnd = NativeMethods.WindowFromPoint(point);
                    NativeMethods.GetWindowThreadProcessId(hWnd, out var processId);
                    var processModule = Process.GetProcessById(processId.ToInt32()).MainModule;
                    if (processModule != null)
                    {
                        var dt = DateTime.Now;
                        WPF.Utils.InvokeTextBox.SetText(fieldOut, $"{dt.Hour}:{dt.Minute}:{dt.Second}  The task is started." + Environment.NewLine);
                        WPF.Utils.InvokeTextBox.AddText(fieldOut, $"{processModule.FileName}");
                    }
                }
            }
            catch (Exception ex)
            {
                var dt = DateTime.Now;
                WPF.Utils.InvokeTextBox.SetText(fieldOut, $"{dt.Hour}:{dt.Minute}:{dt.Second}  Ошибка: {ex.Message}");
                throw;
            }
        }

        #endregion
    }
}
