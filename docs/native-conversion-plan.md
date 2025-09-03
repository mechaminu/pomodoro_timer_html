# Native Conversion Plan

## Framework Options
- **Windows first:** WPF / WinUI 3 (.NET)
  - Small bundles with direct Win32 and Windows Runtime API access
  - Supports background services and global input hooks so the app keeps working when minimized or unfocused
- **Cross-platform:** React Native
  - iOS/Android/Web/Desktop support with relatively small bundle size
  - Background work via `react-native-background-fetch` or similar packages

## Assessment: Web App vs. Native Program

### Pros of staying a Web/PWA
- Zero-install: runs in any modern browser
- Small footprint; straightforward hosting
- Updates delivered instantly to all users

### Cons of staying a Web/PWA
- Browsers throttle or pause timers when tabs lose focus, risking inaccurate timing
- Limited background execution and OS-level integrations such as tray icons or auto-start
- Notifications and wake locks vary across browsers and platforms

### Pros of converting to a Native App
- Full control over background execution; timers continue even when minimized
- Direct access to system APIs for tray icons, notifications, auto-launch, and sound/vibration
- Offline-first by default and easier packaging for app stores or direct distribution

### Cons of converting to a Native App
- Added complexity with build pipelines, platform-specific quirks, and update management
- Larger binaries and higher memory usage
- Need to maintain multiple platform builds and signing processes

## Immediate Steps
1. **Modularize timer logic**
   - Move timer functions from `index.html` into `src/timer.js`
   - Export a reusable API (e.g., `startTimer`, `pauseTimer`, `resetTimer`)
   - Replace inline script in `index.html` with an ES module import
2. **Create Tauri scaffold**
   - Initialize a Tauri project in `native/tauri` using `cargo tauri init`
   - Configure `tauri.conf.json` to point to the existing front-end
   - Ensure build scripts copy `icon.svg` and `maskable-icon.svg` into the Tauri `icons/` directory
3. **Implement background timer & tray integration**
   - In Tauri’s main process, create a system tray icon that keeps the app alive when minimized
   - Move the timer tick loop to a background process, emitting events to the front-end via IPC
   - Add OS notifications and optional vibration/sound hooks for Pomodoro cycle completion
4. **Configure cross-platform builds**
   - Add CI scripts under `.github/workflows/` to build installers for Windows, macOS, and Linux
   - Set up code signing for each platform using environment secrets
   - Document release steps in `README.md`, including platform-specific installation notes

## Longer-term Roadmap
- Configurable Pomodoro/break durations stored locally
- Activity monitoring via keyboard and mouse events to compute a "Focus Metric"
- Real-time status view with graphs and historical analytics
- Ensure operation when the browser is minimized or closed
- Data export, advanced analytics, and platform-specific integrations

