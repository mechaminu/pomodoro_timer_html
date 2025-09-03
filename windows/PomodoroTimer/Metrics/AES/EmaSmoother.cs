using System;

namespace PomodoroTimer.Metrics.AES
{
    /// <summary>
    /// Simple exponential moving average smoother.
    /// </summary>
    public class EmaSmoother
    {
        private readonly double _alpha;
        private bool _hasValue;
        private double _state;

        public EmaSmoother(double alpha)
        {
            _alpha = alpha;
        }

        public double Smooth(double value)
        {
            if (!_hasValue)
            {
                _state = value;
                _hasValue = true;
            }
            else
            {
                _state = _alpha * value + (1 - _alpha) * _state;
            }
            return _state;
        }

        public void Reset()
        {
            _hasValue = false;
            _state = 0;
        }
    }
}
