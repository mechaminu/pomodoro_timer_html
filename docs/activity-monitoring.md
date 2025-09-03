# Activity Monitoring and Focus Metric

This document details how to capture user activity to compute a Focus Metric.

## Goals
- Track keyboard and mouse interactions while the timer is running.
- Derive a score indicating how focused the user was during a session.

## Windows
- Use global hooks via Win32 APIs to record input frequency.
- Respect privacy by limiting data to counts and timestamps.

## React Native
- Utilize `react-native-keyevent` and touch events to gather activity data.
- Store aggregate metrics rather than raw events.

## Next Steps
1. Define algorithm for Focus Metric calculation.
2. Implement platform-specific input listeners.
3. Visualize focus data alongside session history.

[Back to AGENTS](../AGENTS.md)
