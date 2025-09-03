# Activity Monitoring

Captures user input events to calculate a "Focus Metric" that reflects engagement during Pomodoro sessions.

## Windows Implementation
- Listen for global keyboard and mouse events using Windows hooks.
- Count events per interval and correlate with timer state.
- Store aggregated metrics locally for analysis.

## React Native Implementation
- Use `react-native-keyevent` and gesture responders to track input activity.
- Combine event counts with session data to compute the focus metric.
- Persist metrics with `AsyncStorage` or a lightweight database like SQLite.

[Back to AGENTS](../AGENTS.md)
