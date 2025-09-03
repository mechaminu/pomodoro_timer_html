# Background Execution

Ensures the timer continues running and can notify users even when the app is not in focus.

## Windows Implementation
- Provide a tray icon with start/stop controls.
- Optionally run as a background service using `IBackgroundTask`.
- Employ global keyboard shortcuts for quick interaction.

## React Native Implementation
- Integrate `react-native-background-fetch` for periodic wake-ups.
- Use headless JS tasks to handle timer updates when the app is closed.
- Respect platform-specific limitations on background processing.

[Back to AGENTS](../AGENTS.md)
