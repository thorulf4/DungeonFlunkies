using ClientWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace ClientWPF.Scenes.Combat
{
    public class TurnTimer : ViewModel, IDisposable
    {
        public double TimeLeftPct { get; set; }

        private readonly double turnTimeMillis;
        private readonly Stopwatch currentTime;
        private double delayOffset;

        private DispatcherTimer dispatcherTimer;

        public TurnTimer(double turnTimeInMs)
        {
            turnTimeMillis = turnTimeInMs;
            currentTime = Stopwatch.StartNew();

            TimeLeftPct = 100;

            dispatcherTimer = new DispatcherTimer(DispatcherPriority.Background, Application.Current.Dispatcher);
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(50);
            dispatcherTimer.Tick += Update;
            dispatcherTimer.Start();
        }

        public void StartTurn(int sendDelay)
        {
            delayOffset = sendDelay;
            currentTime.Restart();
        }

        private void Update(object sender, EventArgs e)
        {
            double timePassed = Math.Min(turnTimeMillis, (currentTime.ElapsedMilliseconds + delayOffset));

            TimeLeftPct = (1f - timePassed / turnTimeMillis) * 100.0;
            Notify("TimeLeftPct");
        }

        public void Dispose()
        {
            dispatcherTimer.Stop();
            dispatcherTimer = null;
        }

        public void SetEndTime(DateTime turnEnds)
        {
            double timePassed = turnEnds.Subtract(DateTime.Now).TotalMilliseconds;
            currentTime.Restart();
            delayOffset = turnTimeMillis - timePassed;
            TimeLeftPct = (1f - timePassed / turnTimeMillis) * 100.0;
            Notify("TimeLeftPct");
        }
    }
}
