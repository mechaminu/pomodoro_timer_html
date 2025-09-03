using System;
using Timer = System.Timers.Timer;

namespace PomodoroTimer
{
    public class TimerState
    {
        public TimeSpan Remaining { get; set; }
        public TimeSpan Total { get; set; }
        public string Mode { get; set; } = "work";
    }

    public class TimerService
    {
        private readonly Timer _timer = new Timer(250);
        private TimeSpan _workDuration = TimeSpan.FromMinutes(25);
        private TimeSpan _breakDuration = TimeSpan.FromMinutes(5);
        private readonly TimerState _state;

        public event Action<string>? OnComplete;

        public TimerService()
        {
            _state = new TimerState { Total = _workDuration, Remaining = _workDuration };
            _timer.Elapsed += (_, _) => Tick();
        }

        public void Start() => _timer.Start();
        public void Pause() => _timer.Stop();

        public void Reset()
        {
            Pause();
            _state.Remaining = _state.Total;
        }

        public void SetDurations(TimeSpan work, TimeSpan brk)
        {
            _workDuration = work;
            _breakDuration = brk;
            SwitchMode(_state.Mode);
        }

        public TimerState GetState() => _state;

        private void Tick()
        {
            _state.Remaining -= TimeSpan.FromMilliseconds(250);
            if (_state.Remaining <= TimeSpan.Zero)
            {
                _state.Remaining = TimeSpan.Zero;
                Pause();
                OnComplete?.Invoke(_state.Mode);
                SwitchMode(_state.Mode == "work" ? "break" : "work");
                Start();
            }
        }

        private void SwitchMode(string mode)
        {
            _state.Mode = mode;
            _state.Total = mode == "work" ? _workDuration : _breakDuration;
            _state.Remaining = _state.Total;
        }
    }
}
