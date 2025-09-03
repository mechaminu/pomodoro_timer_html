let timerId = null;
let mode = 'work';
let workDuration = 25 * 60;
let breakDuration = 5 * 60;
let total = workDuration;
let remaining = total;

const callbacks = {
  onTick: () => {},
  onModeChange: () => {},
  onComplete: () => {}
};

function configure(opts = {}) {
  Object.assign(callbacks, opts);
}

function startTimer() {
  if (timerId) return;
  timerId = setInterval(tick, 250);
}

function pauseTimer() {
  if (!timerId) return;
  clearInterval(timerId);
  timerId = null;
}

function resetTimer() {
  pauseTimer();
  remaining = total;
  callbacks.onTick(remaining, total, mode);
}

function switchMode(next) {
  mode = next;
  total = mode === 'work' ? workDuration : breakDuration;
  remaining = total;
  callbacks.onModeChange(mode);
  callbacks.onTick(remaining, total, mode);
}

function setDurations(work, brk) {
  if (typeof work === 'number') workDuration = work;
  if (typeof brk === 'number') breakDuration = brk;
  // reset to current mode with new durations
  switchMode(mode);
}

function tick() {
  remaining = Math.max(0, remaining - 0.25);
  callbacks.onTick(remaining, total, mode);
  if (remaining <= 0) {
    callbacks.onComplete(mode);
    switchMode(mode === 'work' ? 'break' : 'work');
    startTimer();
  }
}

function getState() {
  return { timerId, mode, remaining, total, workDuration, breakDuration };
}

export {
  configure,
  startTimer,
  pauseTimer,
  resetTimer,
  switchMode,
  setDurations,
  getState
};
