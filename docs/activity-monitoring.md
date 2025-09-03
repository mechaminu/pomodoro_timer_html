# Activity Monitoring and Focus Metric

[Back to overview](../AGENTS.md)

## Objectives
- Capture keyboard and mouse events to gauge activity.
- Compute "Focus Metric" combining activity frequency and timer status.

## Windows: WPF / WinUI 3
- Hook global input using `SetWindowsHookEx` or `RawInput`.
- Record timestamps of events in background service.
- Aggregate metrics per Pomodoro session.

## Cross-platform: React Native
- Use libraries like `react-native-global-props` or native modules for input hooks (Android only; limited on iOS).
- Capture foreground interaction events as fallback.
- Upload activity samples to JS layer for metric calculations.

## Tasks
1. Design data structure for storing input events.
2. Create focus metric algorithm (e.g., events per minute).
3. Visualize metric in real time and persist historical data.
=======
See the overarching [AGENTS plan](../AGENTS.md). This document covers capturing user activity and deriving a focus metric.

## Objectives
- Capture keyboard and mouse events.
- Combine activity frequency with timer status to compute a focus score.

## Action Steps
1. Hook global keyboard and mouse listeners.
2. Record events alongside timer phases.
3. Calculate a weighted metric representing focus during each session.
4. Store historical metrics for analysis.

## Challenges
- Respecting user privacy and OS security restrictions.
- Normalizing activity levels across different work styles.