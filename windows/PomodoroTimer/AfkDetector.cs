using System;
using System.Runtime.InteropServices;
using System.Timers;

namespace PomodoroTimer
{
    public class AfkDetector : IDisposable
    {
        private readonly Timer _timer = new Timer(1000);
        private TimeSpan _threshold;
        private bool _isAfk;
        public event Action<bool>? AfkStatusChanged;

        public AfkDetector(TimeSpan? threshold = null)
        {
            _threshold = threshold ?? TimeSpan.FromMinutes(1);
            _timer.Elapsed += (_, _) => Check();
        }

        public void Start() => _timer.Start();
        public void Stop() => _timer.Stop();

        public void SetThreshold(TimeSpan threshold) => _threshold = threshold;

        private void Check()
        {
            var idle = GetIdleTime();
            var newStatus = idle > _threshold;
            if (newStatus != _isAfk)
            {
                _isAfk = newStatus;
                AfkStatusChanged?.Invoke(_isAfk);
            }
        }

        public TimeSpan GetIdleTime()
        {
            LASTINPUTINFO info = new LASTINPUTINFO();
            info.cbSize = (uint)Marshal.SizeOf(info);
            if (GetLastInputInfo(ref info))
            {
                uint idle = (uint)Environment.TickCount - info.dwTime;
                return TimeSpan.FromMilliseconds(idle);
            }
            return TimeSpan.Zero;
        }

        public void Dispose() => _timer.Dispose();

        private struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }

        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
    }
}
