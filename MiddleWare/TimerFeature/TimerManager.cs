namespace MiddleWare.TimerFeature
{
    public class TimerManager
    {
        private Timer timer;
        private AutoResetEvent AutoResetEvent;
        private Action action;
        public DateTime TimerStarted { get; set; }
        public TimerManager() { }
        public TimerManager(Action action)
        {
            this.action = action;
            this.AutoResetEvent = new AutoResetEvent(false);
            timer = new Timer(Execute, AutoResetEvent, 10000, 30000);
            TimerStarted = DateTime.Now;    
        }
        public void TimerManagerOption1(Action action)
        {
            this.action = action;
            this.AutoResetEvent = new AutoResetEvent(false);
            timer = new Timer(Execute, AutoResetEvent, 1000, 5000);
            TimerStarted = DateTime.Now;
        }
        public void Execute(object StateInfo)
        {
            action();
            if((DateTime.Now - TimerStarted).TotalSeconds > 60000000)
            {
                timer.Dispose();
            }
        }


    }
}
