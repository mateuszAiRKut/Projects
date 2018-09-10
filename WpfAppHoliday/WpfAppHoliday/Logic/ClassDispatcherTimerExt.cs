using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WpfAppHoliday.Logic
{
    class ClassDispatcherTimerExt  : DispatcherTimer
    {
        public bool IsReentrant { get; set; }
        public bool IsRunning { get; private set; }
        public Func<Task> TickTask { get; set; }

        public ClassDispatcherTimerExt()
        {
            Tick += SmartDispatcherTimer_Tick;
        }

        async void SmartDispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (TickTask == null)
            {
                System.Windows.MessageBox.Show("No task set!");
                return;
            }

            if (IsRunning && !IsReentrant)
            {
                System.Windows.MessageBox.Show("Task already running");
                return;
            }

            try
            {
                IsRunning = true;
                System.Windows.MessageBox.Show("Running Task");
                await TickTask.Invoke();
                System.Windows.MessageBox.Show("Task Completed");
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Task Failed");
            }
            finally
            {
                IsRunning = false;
            }
        }
    }
}
