using System;
using System.Collections.Generic;
using System.Linq;

namespace PomodoroTimer.Metrics.AES
{
    /// <summary>
    /// Maintains a rolling window of values and provides P90 based normalization.
    /// </summary>
    public class RollingQuantileNormalizer
    {
        private readonly int _windowSize;
        private readonly Queue<double> _values = new();

        public RollingQuantileNormalizer(int windowSize)
        {
            _windowSize = windowSize;
        }

        public double Normalize(double value)
        {
            _values.Enqueue(value);
            if (_values.Count > _windowSize)
                _values.Dequeue();

            var arr = _values.ToArray();
            Array.Sort(arr);
            int idx = (int)Math.Ceiling(0.9 * arr.Length) - 1;
            double p90 = idx >= 0 ? arr[idx] : 0.0;
            if (p90 <= 0)
                return 0.0;
            var norm = value / p90;
            return Math.Max(0.0, Math.Min(1.0, norm));
        }
    }
}
