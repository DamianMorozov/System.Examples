using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinForms.Utils;

// ReSharper disable IdentifierTypo

namespace WinForms.System.MemoryLimit
{
    /// <summary>
    /// Task memory.
    /// </summary>
    public class MemoryAsyncTask : IDisposable
    {
        public bool IsExecute { get; private set; }
        public bool IsDisposed { get; private set; }
        public bool IsLimitOverload { get; private set; }
        private int SleepMiliSeconds { get; }
        private int WaitCloseMiliSeconds { get; }
        private bool SimulateAction { get; }

        private RichTextBox _richTextBox;
        private MemorySize _objectMemorySize;
        private readonly EnumMemoryLimitAction _enumMemoryLimitAction;

        public MemoryAsyncTask(int getSleepMiliSeconds, int getWaitCloseMiliSeconds, ulong getLimitBytes,
            int getEnumMemoryLimitAction, ref RichTextBox richTextBox, bool simulateAction)
        {
            IsExecute = true;
            IsDisposed = false;
            IsLimitOverload = false;
            SleepMiliSeconds = getSleepMiliSeconds;
            WaitCloseMiliSeconds = getWaitCloseMiliSeconds;

            _richTextBox = richTextBox;
            _objectMemorySize = new MemorySize(getLimitBytes);

            switch (getEnumMemoryLimitAction)
            {
                case 1:
                    _enumMemoryLimitAction = EnumMemoryLimitAction.Restart;
                    break;
                default:
                    _enumMemoryLimitAction = EnumMemoryLimitAction.Exit;
                    break;
            }

            SimulateAction = simulateAction;
        }

        public Task Execute()
        {
            return Task.Run(() =>
            {
                CheckIfDisposed();

                try
                {
                    var limitBytes = _objectMemorySize.Limit.Bytes;
                    var limitMegaBytes = _objectMemorySize.Limit.GetMegaBytes();

                    while (IsExecute)
                    {
                        _objectMemorySize.Physical.Bytes = (ulong) Process.GetCurrentProcess().WorkingSet64;
                        _objectMemorySize.Virtual.Bytes = (ulong) Process.GetCurrentProcess().PrivateMemorySize64;
                        IsLimitOverload = !(_objectMemorySize.Physical.Bytes < limitBytes || limitBytes <= 0);

                        if (_richTextBox != null)
                            InvokeControl.SetText(_richTextBox,
                                @"Task memory ID:  " + Task.CurrentId + Environment.NewLine +
                                (limitMegaBytes <= 0 
                                    ? @"Limit memory:    " + @"0 MB - is not set"
                                    : @"Limit memory:    " + $"{limitMegaBytes:N0}" + @" MB - is set") + Environment.NewLine +
                                @"Physical memory: " + $"{_objectMemorySize.Physical.GetMegaBytes():N0}" + @" MB" + Environment.NewLine +
                                @"Virtual memory:  " + $"{_objectMemorySize.Virtual.GetMegaBytes():N0}" + @" MB" + Environment.NewLine +
                                (SimulateAction ? @"Simulate mode is enabled." : @"Simulate mode is disabled."));
                        Thread.Sleep(SleepMiliSeconds);

                        if (IsLimitOverload && !SimulateAction)
                        {
                            Dispose();
                            switch (_enumMemoryLimitAction)
                            {
                                case EnumMemoryLimitAction.Restart:
                                    MessageBox.Show(@"Memory limit is exceeded!" + Environment.NewLine + @"Application will be restart.",
                                        Application.ProductName);
                                    Application.Restart();
                                    break;
                                default:
                                    MessageBox.Show(@"Memory limit is exceeded!" + Environment.NewLine + @"Application will be exit.",
                                        Application.ProductName);
                                    Application.Exit();
                                    break;
                            }
                        }
                    }
                    InvokeControl.SetText(_richTextBox, @"Task memory is finished.");
                }
                catch (Exception exception)
                {
                    InvokeControl.SetText(_richTextBox, @"Error execute Task memory! " + Environment.NewLine + exception.Message);
                }
                finally
                {
                    IsExecute = false;
                }
            });
        }

        public void Stop()
        {
            IsExecute = false;
        }

        ~MemoryAsyncTask()
        {
            Dispose();
        }

        private void DisposeManagedResources()
        {
            try
            {
                Stop();

                // Timer
                var dtStart = DateTime.Now;
                var timeSpan = new TimeSpan();
                while (IsExecute && timeSpan.TotalMilliseconds <= WaitCloseMiliSeconds)
                {
                    Application.DoEvents();
                    Thread.Sleep(25);
                    timeSpan = DateTime.Now - dtStart;
                }
            }
            catch (Exception exception)
            {
                InvokeControl.SetText(_richTextBox, @"Error DisposeManagedResources! " + Environment.NewLine + exception.Message);
            }
            finally
            {
                _objectMemorySize = null;
                _richTextBox = null;
            }
        }

        private void DisposeUnmanagedResources()
        {
        }

        public virtual void Dispose()
        {
            lock (this)
            {
                if (!IsDisposed)
                {
                    // Dispose managed resources
                    DisposeManagedResources();

                    // Dispose unmanaged resources
                    DisposeUnmanagedResources();

                    // Log
                    InvokeControl.SetText(_richTextBox, @"Task memory is disposed.");

                    // Dispose flag
                    IsDisposed = true;
                }

                // Disable the garbage collector from calling the destructor
                GC.SuppressFinalize(this);
            }
        }

        private void CheckIfDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(this + @": object has been disposed!");
            }
        }
    }
}
