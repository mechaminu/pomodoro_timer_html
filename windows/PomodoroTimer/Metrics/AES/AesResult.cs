using System;

namespace PomodoroTimer.Metrics.AES
{
    /// <summary>
    /// Holds computed scores for a single second.
    /// </summary>
    public class AesResult
    {
        public double InputIntensityRaw { get; set; }
        public double InputIntensitySmooth { get; set; }
        public double ContextScoreRaw { get; set; }
        public double ContextScoreSmooth { get; set; }
        public double StabilityScoreRaw { get; set; }
        public double StabilityScoreSmooth { get; set; }
        public double Aes { get; set; }
    }
}
