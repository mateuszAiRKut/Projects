using System.Threading;

namespace WpfAlarmClock
{
    public delegate void MyDlegateTimer();
    public class ClassClock
    {
        public ref byte Hour { get { return ref hour; } }
        public ref byte Minute { get { return ref minute; } }
        public ref byte Second { get { return ref second; } }

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
                /*timerClock = new DispatcherTimer(); //sposob 2
                timerClock.Interval = TimeSpan.FromSeconds(1);
                timerClock.Tick += (s,e) => { delegateClock(); };
                timerClock.Start();*/
                threadClock = new Thread(() => { delegateClock(); }); // sposob 1
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


        public MyDlegateTimer delegateClock; //ta delegata jest opakowaniem dla ThreadStart
        private Thread threadClock; // sposob 1
        //private DispatcherTimer timerClock; //sposob 2
        private bool isActive;
        private byte hour, minute, second;
    }
}
