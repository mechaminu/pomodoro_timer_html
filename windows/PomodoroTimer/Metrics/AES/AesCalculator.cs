using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace PomodoroTimer.Metrics.AES
{
    /// <summary>
    /// Computes per-second Active Engagement Score (AES) from interaction signals.
    /// Simplified implementation of the specification in AGENTS instructions.
    /// </summary>
    public class AesCalculator
    {
        private readonly RollingQuantileNormalizer _keyboardNorm;
        private readonly RollingQuantileNormalizer _mouseNorm;
        private readonly EmaSmoother _iSmooth;
        private readonly EmaSmoother _cSmooth;
        private readonly EmaSmoother _sSmooth;

        private readonly Queue<int> _switchWindow = new();
        private readonly Queue<int> _notificationWindow = new();
        private readonly Queue<bool> _microIdleWindow = new();
        private string? _lastContext;

        private static readonly (Regex pattern, double score)[] _rules = new[]
        {
            (new Regex("ide|code|matlab|pycharm|vscode|clion", RegexOptions.IgnoreCase), 1.0),
            (new Regex("docs|latex|notion|obsidian|word|powerpoint|excel|onenote", RegexOptions.IgnoreCase), 0.5),
            (new Regex("youtube|tiktok|insta|reddit|twitter|x.com|netflix", RegexOptions.IgnoreCase), -1.0)
        };

        public AesCalculator(int normWindowSeconds = 120 * 60, double emaAlpha = 0.15)
        {
            _keyboardNorm = new RollingQuantileNormalizer(normWindowSeconds);
            _mouseNorm = new RollingQuantileNormalizer(normWindowSeconds);
            _iSmooth = new EmaSmoother(emaAlpha);
            _cSmooth = new EmaSmoother(emaAlpha);
            _sSmooth = new EmaSmoother(emaAlpha);
        }

        public AesResult Process(DateTime timestamp, SignalSample sample)
        {
            // Normalization
            var keyboardNorm = _keyboardNorm.Normalize(sample.KCommit);
            var editPenalized = Math.Max(0, sample.KCommit - 0.5 * sample.KEdit);
            var editNorm = _keyboardNorm.Normalize(editPenalized);
            var mouseComposite = sample.MMovePx + 60 * sample.MClick + 12 * sample.MScrollNotches;
            var mouseNorm = _mouseNorm.Normalize(mouseComposite);

            // Input intensity
            double inputRaw = 0.55 * keyboardNorm + 0.15 * editNorm + 0.30 * mouseNorm;
            double inputSmooth = _iSmooth.Smooth(inputRaw);

            // Context score
            double contextRaw = ScoreContext(sample);
            double contextSmooth = _cSmooth.Smooth(contextRaw);

            // Stability score components
            UpdateStability(sample);
            double switchRate = _switchWindow.Sum() / 10.0;
            double notificationsNorm = Math.Min(1.0, _notificationWindow.Sum() / 10.0);
            double microIdleRate = _microIdleWindow.Count > 0 ? _microIdleWindow.Count(b => b) / 20.0 : 0.0;
            double stabilityRaw = 1 - 0.5 * switchRate - 0.3 * notificationsNorm - 0.4 * microIdleRate;
            stabilityRaw = Math.Max(0.0, Math.Min(1.0, stabilityRaw));
            double stabilitySmooth = _sSmooth.Smooth(stabilityRaw);

            // AES
            double aes = 100 * (0.55 * inputSmooth + 0.25 * contextSmooth + 0.20 * stabilitySmooth);
            aes = Math.Max(0, Math.Min(100, aes));

            return new AesResult
            {
                InputIntensityRaw = inputRaw,
                InputIntensitySmooth = inputSmooth,
                ContextScoreRaw = contextRaw,
                ContextScoreSmooth = contextSmooth,
                StabilityScoreRaw = stabilityRaw,
                StabilityScoreSmooth = stabilitySmooth,
                Aes = aes
            };
        }

        private double ScoreContext(SignalSample sample)
        {
            string key = sample.UrlDomain ?? sample.ForegroundAppId ?? string.Empty;
            foreach (var (pattern, score) in _rules)
            {
                if (pattern.IsMatch(key))
                    return score;
            }
            return 0.0; // unknown
        }

        private void UpdateStability(SignalSample sample)
        {
            string ctx = sample.UrlDomain ?? sample.ForegroundAppId ?? string.Empty;
            bool switched = _lastContext != null && ctx != _lastContext;
            _lastContext = ctx;

            Enqueue(_switchWindow, switched ? 1 : 0, 10);
            Enqueue(_notificationWindow, sample.NotificationsCount, 10);
            Enqueue(_microIdleWindow, sample.MicroIdle, 20);
        }

        private static void Enqueue<T>(Queue<T> q, T value, int max)
        {
            q.Enqueue(value);
            while (q.Count > max)
                q.Dequeue();
        }
    }
}
