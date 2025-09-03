using System;

namespace PomodoroTimer.Metrics.AES
{
    /// <summary>
    /// Represents aggregated per-second interaction signals used for AES computation.
    /// </summary>
    public class SignalSample
    {
        public double KCommit { get; set; }
        public double KEdit { get; set; }
        public double KNav { get; set; }
        public double MMovePx { get; set; }
        public double MClick { get; set; }
        public double MScrollNotches { get; set; }
        public string? ForegroundAppId { get; set; }
        public string? UrlDomain { get; set; }
        public bool MicroIdle { get; set; }
        public bool MacroIdle { get; set; }
        public int NotificationsCount { get; set; }
    }
}
