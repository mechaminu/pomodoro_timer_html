# AGENTS.md

## Framework Summary

- **Windows first**: WPF / WinUI 3 (.NET)
  - Small bundles, direct Win32 and Windows Runtime API access
  - Supports background services and global input hooks so the app keeps working when minimized or unfocused
- **Cross-platform**: React Native
  - iOS/Android/Web/Desktop support with relatively small bundle size
  - Background work via `react-native-background-fetch` or similar packages
  - Leverages existing JavaScript/TypeScript and React skills

## Implementation Plan

1. **Core Timer**
   - Configurable Pomodoro/Break durations stored locally
   - Timer state and notifications

2. **Background Execution**
   - Windows: tray app or background service
   - React Native: use background fetch/task libraries

3. **Activity Monitoring and Focus Metric**
   - Capture keyboard and mouse events
   - Combine event frequency with timer status to produce a "Focus Metric"

4. **Visualization**
   - Real-time status view with graphs and stats
   - Persist and analyze historical data

5. **Future Enhancements**
   - Ensure operation when browser minimized or closed
   - Data export, advanced analytics, and platform-specific integrations

## Detailed Action Plan

Detailed documentation for each step is available in `/docs`:

- [Core Timer](docs/core-timer.md)
- [Background Execution](docs/background-execution.md)
- [Activity Monitoring and Focus Metric](docs/activity-monitoring.md)
- [Visualization](docs/visualization.md)
- [Future Enhancements](docs/future-enhancements.md)
