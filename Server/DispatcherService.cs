using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class DispatcherService
    {
        private Thread thread;

        SortedList<long, Action> callbacks;

        public DispatcherService()
        {
            StartThread();
        }

        public QueuedInvocation InvokeIn(long timeInMs, Action callback)
        {
            long time = GetSystemTime() + timeInMs;

            if (callbacks.ContainsKey(time))
                return InvokeIn(timeInMs + 1, callback);

            lock (callbacks)
            {
                callbacks.Add(time, callback);
            }

            return new QueuedInvocation(this, time);
        }

        private long GetSystemTime()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        private void StartThread()
        {
            Task.Run(() =>
            {
                callbacks = new SortedList<long, Action>();
                thread = new Thread(HandleDispatching);
                thread.IsBackground = true;
                thread.Start();
            });
        }

        private void HandleDispatching(object obj)
        {
            while (true)
            {
                lock (callbacks)
                {
                    if (callbacks.Count > 0 && GetSystemTime() >= callbacks.Keys.First())
                    {
                        callbacks.Values[0].Invoke();
                        callbacks.RemoveAt(0);
                    }
                }
            }

        }

        public class QueuedInvocation
        {
            private DispatcherService dispatcher;
            private long uniqueId;

            public QueuedInvocation(DispatcherService dispatcherService, long time)
            {
                dispatcher = dispatcherService;
                uniqueId = time;
            }

            public void Cancel()
            {
                lock (dispatcher.callbacks)
                {
                    dispatcher.callbacks.Remove(uniqueId);
                }
            }
        }
    }
}
