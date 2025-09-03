using System;
using System.Collections.Generic;
using System.Linq;

namespace PomodoroTimer.Metrics.AES
{
    /// <summary>
    /// Detects focus spans from AES values using simple threshold rules.
    /// </summary>
    public class FocusSpanDetector
    {
        private readonly int _enterThreshold;
        private readonly int _exitThreshold;
        private readonly int _enterMinDuration;
        private readonly int _exitMinDuration;

        private bool _inSpan;
        private DateTime _spanStart;
        private readonly List<double> _currentAes = new();
        private readonly List<double> _currentContextScores = new();
        private readonly List<bool> _currentIdleFlags = new();
        private int _aboveCounter;
        private int _belowCounter;
        private readonly List<FocusSpan> _spans = new();

        public IEnumerable<FocusSpan> Spans => _spans;

        public FocusSpanDetector(int enterThreshold = 60, int exitThreshold = 40, int enterMinDuration = 12, int exitMinDuration = 25)
        {
            _enterThreshold = enterThreshold;
            _exitThreshold = exitThreshold;
            _enterMinDuration = enterMinDuration;
            _exitMinDuration = exitMinDuration;
        }

        public void AddSample(DateTime ts, AesResult metrics, SignalSample sample)
        {
            double aes = metrics.Aes;

            if (!_inSpan)
            {
                if (aes >= _enterThreshold)
                {
                    _aboveCounter++;
                    if (_aboveCounter >= _enterMinDuration)
                    {
                        _inSpan = true;
                        _spanStart = ts - TimeSpan.FromSeconds(_enterMinDuration - 1);
                        _currentAes.Clear();
                        _currentContextScores.Clear();
                        _currentIdleFlags.Clear();
                    }
                }
                else
                {
                    _aboveCounter = 0;
                }
            }
            else
            {
                _currentAes.Add(aes);
                _currentContextScores.Add(metrics.ContextScoreSmooth);
                _currentIdleFlags.Add(sample.KCommit == 0);

                if (aes <= _exitThreshold)
                {
                    _belowCounter++;
                    if (_belowCounter >= _exitMinDuration)
                    {
                        EndSpan(ts);
                    }
                }
                else
                {
                    _belowCounter = 0;
                }

                if (sample.MacroIdle || metrics.ContextScoreSmooth < 0)
                {
                    EndSpan(ts);
                }
            }
        }

        private void EndSpan(DateTime ts)
        {
            if (!_inSpan) return;
            _inSpan = false;
            var end = ts;
            int duration = (int)Math.Max(1, (end - _spanStart).TotalSeconds);
            var span = new FocusSpan
            {
                Start = _spanStart,
                End = end,
                DurationSeconds = duration,
                AvgAes = _currentAes.Count > 0 ? _currentAes.Average() : 0,
                MedianAes = _currentAes.Count > 0 ? _currentAes.OrderBy(v => v).ElementAt(_currentAes.Count / 2) : 0,
                ProductivePct = _currentContextScores.Count > 0 ? _currentContextScores.Count(v => v > 0.75) * 100.0 / _currentContextScores.Count : 0,
                IdlePct = _currentIdleFlags.Count > 0 ? _currentIdleFlags.Count(b => b) * 100.0 / _currentIdleFlags.Count : 0
            };
            _spans.Add(span);
            _aboveCounter = 0;
            _belowCounter = 0;
        }
    }
}
