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
=======
Linked with [AGENTS.md](../AGENTS.md), this document outlines how to present real‑time and historical timer data.

## Objectives
- Display current timer status with progress indicators.
- Provide graphs and statistics for past sessions.

## Action Steps
1. Build a dashboard component that reflects live timer state.
2. Render charts (e.g., line or bar graphs) of focus metrics and session history.
3. Allow filtering by date ranges or session types.
4. Ensure accessibility with readable colors and labels.

## Challenges
- Efficiently rendering graphs on resource‑constrained devices.
- Designing a clean UI that works across screen sizes.