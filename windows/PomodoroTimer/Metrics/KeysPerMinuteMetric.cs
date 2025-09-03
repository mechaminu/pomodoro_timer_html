using System;
using System.Collections.Generic;

namespace PomodoroTimer.Metrics
{
    public class KeysPerMinuteMetric : IActivityMetric
    {
        private readonly TimeSpan _window;
        private readonly Queue<DateTime> _timestamps = new();

        public KeysPerMinuteMetric(TimeSpan? window = null)
        {
            _window = window ?? TimeSpan.FromSeconds(10);
        }

        public double CurrentValue
        {
            get
            {
                PruneOld(DateTime.Now);
                return _timestamps.Count / _window.TotalMinutes;
            }
        }

        public void RecordEvent(DateTime timestamp)
        {
            _timestamps.Enqueue(timestamp);
            PruneOld(timestamp);
        }

        public void Tick(DateTime timestamp)
        {
            PruneOld(timestamp);
        }

        public void Reset()
        {
            _timestamps.Clear();
        }

        private void PruneOld(DateTime reference)
        {
            while (_timestamps.Count > 0 && reference - _timestamps.Peek() > _window)
            {
                _timestamps.Dequeue();
            }
        }
    }
}
