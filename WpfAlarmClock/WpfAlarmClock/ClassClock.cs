using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace WpfAlarmClock
{
    delegate void MyDlegateTimer();
    class ClassClock
    {
        public ref byte Hour { get { return ref hour; } }
        public ref byte Minute { get { return ref minute; } }
        public ref byte Second { get { return ref second; } }

        public ClassClock()
        {
           
        }

        public void AddActionClock(MyDlegateTimer action) //Action
        {
            delegateClock += action;
        }

        public void StartClock()
        {
            if (delegateClock == null)
                return;
            if(!isActive)
            {
                isActive = true;
                /*timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += timer_Tick;
                timer.Start();*/
                threadClock = new Thread(() => { delegateClock(); });
                threadClock.Priority = ThreadPriority.Highest;
                threadClock.Start();
            }

        }

        public void StopClock()
        {
            isActive = false;
            threadClock.Abort();
            delegateClock = null;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        /*void timer_Tick(object sender, EventArgs e)
        {
            hour = (byte)DateTime.Now.Hour;
            minute = (byte)DateTime.Now.Minute;
            second = (byte)DateTime.Now.Second;
            actualT.Clear();
            actualT.Append(hour).Append(":").Append(minute).Append(":").Append(second);
            actualTime.Text = actualT.ToString();
        }*/

        private MyDlegateTimer delegateClock;
        private Thread threadClock;
        private bool isActive;
        private byte hour, minute, second;
        //private Action objectD; ThreadStart objectD;
    }
}
