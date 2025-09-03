using System;

namespace PomodoroTimer.Metrics.AES
{
    /// <summary>
    /// Wraps <see cref="AesCalculator"/> into an <see cref="IActivityMetric"/> implementation
    /// for use within the main timer application. Keyboard commits are counted per second
    /// and converted into an AES value.
    /// </summary>
    public class AesMetric : PomodoroTimer.Metrics.IActivityMetric
    {
        private readonly AesCalculator _calculator = new();
        private int _commitCount;
        private DateTime _lastTick = DateTime.MinValue;

        public double CurrentValue { get; private set; }

        public void RecordEvent(DateTime timestamp)
        {
            _commitCount++;
        }

        public void Tick(DateTime timestamp)
        {
            if (_lastTick == DateTime.MinValue)
                _lastTick = timestamp;
            if ((timestamp - _lastTick).TotalSeconds >= 1)
            {
                var sample = new SignalSample
                {
                    KCommit = _commitCount,
                    KEdit = 0,
                    KNav = 0,
                    MMovePx = 0,
                    MClick = 0,
                    MScrollNotches = 0,
                    ForegroundAppId = null,
                    UrlDomain = null,
                    MicroIdle = _commitCount == 0,
                    NotificationsCount = 0
                };
                var result = _calculator.Process(timestamp, sample);
                CurrentValue = result.Aes;
                _commitCount = 0;
                _lastTick = timestamp;
            }
        }

        public void Reset()
        {
            _commitCount = 0;
            CurrentValue = 0;
            _lastTick = DateTime.MinValue;
        }
    }
}
