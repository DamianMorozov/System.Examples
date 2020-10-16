using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinForms.Utils;

namespace WinForms.MemoryLimit
{
    public partial class Form1 : Form
    {
        private MemoryAsyncTask _objectAsyncTaskMemory;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBoxMemoryLimit.SelectedIndex = 1;
            // GUI access
            GuiEnable(true);
        }

        private async void buttonCreate_Click(object sender, EventArgs e)
        {
            // GUI access
            GuiEnable(false);

            if (_objectAsyncTaskMemory == null)
            {
                // Create object
                _objectAsyncTaskMemory = new MemoryAsyncTask(
                    500, 5000, Convert.ToUInt64(numericUpDownMemoryLimit.Value * 1048576),
                    comboBoxMemoryLimit.SelectedIndex, ref richTextBoxTask, checkBoxSimulateMode.Checked);

                // Create task
                using (var taskMemory = Task.WhenAll(_objectAsyncTaskMemory.Execute()))
                {
                    // Wait task execute
                    await taskMemory;
                }

                // Release object
                _objectAsyncTaskMemory = null;

                // GUI access
                GuiEnable(true);
            }
        }

        // GUI access
        private void GuiEnable(bool enable)
        {
            checkBoxMemoryLimit.Enabled = enable;
            checkBoxSimulateMode.Enabled = enable;
            numericUpDownMemoryLimit.Enabled = enable;
            comboBoxMemoryLimit.Enabled = enable;
        }

        private void buttonDispose_Click(object sender, EventArgs e)
        {
            _objectAsyncTaskMemory?.Stop();
        }

        private async void buttonMoreMemory_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                InvokeControl.SetText(richTextBoxMoreMemory, string.Empty);
                var str = string.Empty;
                for (var i = 1; i <= Convert.ToInt32(numericUpDown.Value * 1024); i++)
                {
                    str += i + @" ";
                }
                InvokeControl.AddText(richTextBoxMoreMemory, str);
            });
        }

        private void numericUpDownMemoryLimit_ValueChanged(object sender, EventArgs e)
        {
            checkBoxMemoryLimit.Checked = (int)numericUpDownMemoryLimit.Value != 0;
        }
    }
}
