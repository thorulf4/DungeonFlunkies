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
        public float TimeLeftPct { get; set; }

        private readonly long turnTimeMillis;
        private readonly Stopwatch currentTime;
        private int delayOffset;

        private DispatcherTimer dispatcherTimer;

        public TurnTimer(long turnTimeInMs)
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
            float timePassed = MathF.Min(turnTimeMillis, (currentTime.ElapsedMilliseconds + delayOffset));

            TimeLeftPct = (1f - timePassed / turnTimeMillis) * 100f;
            Notify("TimeLeftPct");
        }

        public void Dispose()
        {
            dispatcherTimer.Stop();
            dispatcherTimer = null;
        }
    }
}
