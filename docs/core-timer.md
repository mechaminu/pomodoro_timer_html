# Core Timer

Part of the [AGENTS plan](../AGENTS.md), this document outlines how to implement the Pomodoro and break timers natively.

## Objectives
- Store configurable Pomodoro and break durations locally.
- Provide start, pause, and reset controls.
- Persist timer state so sessions survive app restarts.

## Action Steps
1. Define timer models and persistence (local storage or SQLite).
2. Implement a foreground timer loop with notification hooks.
3. Expose timer state through a view‑model or context for UI components.
4. Add unit tests for timing accuracy and state transitions.

## Challenges
- Ensuring accuracy across sleep or background states.
- Handling user‑initiated changes mid‑session without losing progress.
