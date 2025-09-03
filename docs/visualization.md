# Visualization

[Back to overview](../AGENTS.md)

## Objectives
- Provide real-time status, graphs, and statistics.
- Display historical data across sessions.

## Windows: WPF / WinUI 3
- Use `Graph` or `Chart` controls for timeline views.
- Bind timer and activity data via MVVM pattern.
- Implement data persistence with `SQLite` or local file.

## Cross-platform: React Native
- Render charts using libraries such as `victory-native` or `react-native-svg`.
- Leverage Redux or Context for state management.
- Store history with `SQLite` (via `react-native-sqlite-storage`) or local files.

## Tasks
1. Define data models for sessions and metrics.
2. Build components for real-time progress and summaries.
3. Implement export function for session history.
