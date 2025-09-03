using System;
using System.Collections.Generic;
using System.Linq;

namespace PomodoroTimer.Metrics.AES
{
    public class FocusSpan
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int DurationSeconds { get; set; }
        public double AvgAes { get; set; }
        public double MedianAes { get; set; }
        public double ProductivePct { get; set; }
        public double IdlePct { get; set; }
    }
}
