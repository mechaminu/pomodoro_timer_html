# Core Timer

This document describes the shared timer logic needed by all native clients.

## Features
- Configurable work and break durations persisted locally.
- Start, pause, resume, and reset operations.
- Emits events when sessions start and complete.

## Implementation Notes
- Encapsulate logic in a platform-agnostic module (C# class library for Windows, JS module for React Native).
- Provide hooks for UI components to subscribe to timer updates.
- Expose serialization helpers for persisting state between app launches.

## Next Steps
1. Define timer state machine and interfaces.
2. Write unit tests for state transitions.
3. Integrate with platform-specific storage and notification systems.

[Back to AGENTS](../AGENTS.md)
