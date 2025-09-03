using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Timers;

namespace PomodoroTimer
{
    public class FocusTracker : IDisposable
    {
        private readonly Timer _timer = new Timer(1000);
        private IntPtr _lastWindow = IntPtr.Zero;
        private DateTime _windowStart;
        private readonly Dictionary<string, TimeSpan> _focusTime = new();

        public FocusTracker()
        {
            _timer.Elapsed += (_, _) => Tick();
        }

        public void Start()
        {
            _windowStart = DateTime.Now;
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
            RecordWindowTime();
        }

        private void Tick()
        {
            var hwnd = GetForegroundWindow();
            if (hwnd != _lastWindow)
            {
                RecordWindowTime();
                _lastWindow = hwnd;
                _windowStart = DateTime.Now;
            }
        }

        private void RecordWindowTime()
        {
            if (_lastWindow == IntPtr.Zero) return;
            if (DateTime.Now <= _windowStart) return;
            uint pid;
            GetWindowThreadProcessId(_lastWindow, out pid);
            try
            {
                var proc = Process.GetProcessById((int)pid);
                var name = proc.ProcessName;
                var duration = DateTime.Now - _windowStart;
                if (_focusTime.ContainsKey(name))
                    _focusTime[name] += duration;
                else
                    _focusTime[name] = duration;
            }
            catch { }
        }

        public TimeSpan GetFocusTime(string processName)
        {
            return _focusTime.TryGetValue(processName, out var t) ? t : TimeSpan.Zero;
        }

        public void Dispose()
        {
            Stop();
            _timer.Dispose();
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);
    }
}
