/*
ECX UTILITY SCRIPTS
Timer
A versatile and useful Timer script that is incorporated within the Unity Time tracking (unlike C# System.Timers.Timer which runs on the system clock external to Unity and will cause problems for game timing).  Handles its own updates and auto-destruction when complete.
To create a timer, simply use Timer.Create (for a single-use) or Timer.CreatePersistent and provide the necessary parameters.
Subscribe to events like onComplete, onUpdate, onMillisecond, etc as needed via `timerName.onMillisecond += MyMethod;`
Last updated: Mar 16, 2024
*/

using System;
using UnityEngine;

namespace EcxUtilities {
	public class Timer {
	#region Fields / Properties
		public float duration { get; private set; }
		public event Action onComplete;	// Subscribe for an event on timer completion.
		public event Action<float> onUpdate;	// Subscribe for an event on every timer tick (required float parameter returns time remaining).
		public event Action onMillisecond;	// Subscribe for an event every millisecond (e.g. play a tick for a stopwatch).
		public event Action onSecond;	// Subscribe for an event every second (e.g. to beep every second on a 3 sec "get ready" timer).
		public event Action onMinute;	// Subscribe for an event every minute (e.g. for a 5 minute countdown).
		public bool isRepeating { get; private set; }
		public bool isPersistent { get; private set; }
		public bool isRealtime { get; private set; }	// Uses unscaled time (essentially realtime)
		public bool isPaused { get; private set; }
		public bool isComplete { get; private set; }
		public float startTime { get; private set; }
		public float completeTime { get; private set; }
		public float remaining { get; private set; }
		public float elapsed => duration - remaining;
		public float fractionRemaining => remaining / duration;
		public float fractionElapsed => elapsed / duration;
		private float prevMillisecondFired = 999;
		private float prevSecondFired = 999;
		private float prevMinuteFired = 999;
	#endregion Fields / Properties

	#region Constructor / Destructor
		private Timer(float duration, Action onComplete, Action<float> onUpdate, bool isRealtime, bool isRepeating, bool isPersistent) {
			this.duration = duration;
			this.onComplete = onComplete;
			this.onUpdate = onUpdate;
			this.isRealtime = isRealtime;
			this.isRepeating = isRepeating;
			this.isPersistent = isPersistent;
			isPaused = false;
			isComplete = false;
			startTime = isRealtime ? Time.unscaledTime : Time.time;
			completeTime = startTime + completeTime;
			remaining = duration;
			TimerManager.Instance.AddTimer(this);
			Start();
		}

		~Timer() =>
			Dispose();
	#endregion Constructor / Destructor
	
	#region Methods
		/// <summary>
		/// Creates a new single-use (destroyed on complete) countdown timer based on Unity game time (unlike C# System.Timers.Timer which runs on the system clock external to Unity and will cause problems for game timing).  This clock is affected by Time.timeScale. If you need to ignore timeScale effects, set useUnscaledTime to true.
		/// </summary>
		/// <param name="duration">The duration in seconds</param>
		/// <param name="onComplete">Method that is called when timer is complete (takes no arguments)</param>
		/// <param name="onUpdate">Optional: Method that is called on each update. Takes float parameter which provides time remaining</param>
		/// <param name="isRealtime">Optional: Use unscaled time which ignores changes to Time.timeScale (essentially realtime)</param>
		/// <returns></returns>
		public static Timer Create(float duration, Action onComplete, Action<float> onUpdate=null, bool isRealtime=false) =>
			new Timer(duration, onComplete, onUpdate, isRealtime, false, false);

		/// <summary>
		/// Creates a new persistent countdown timer based on Unity game time (unlike C# System.Timers.Timer which runs on the system clock external to Unity and will cause problems for game timing).  This clock is affected by Time.timeScale. If you need to ignore timeScale effects, set useUnscaledTime to true.
		/// </summary>
		/// <param name="duration">The duration in seconds</param>
		/// <param name="onComplete">Method that is called when timer is complete (takes no arguments)</param>
		/// <param name="onUpdate">Optional: Method that is called on each update. Takes float parameter which provides time remaining</param>
		/// <param name="isRealtime">Optional: Use unscaled time which ignores changes to Time.timeScale (essentially realtime)</param>
		/// <param name="isRepeating">Optional: Set to true if you want this timer to repeat on completion</param>
		/// <returns></returns>
		public static Timer CreatePersistent(float duration, Action onComplete, Action<float> onUpdate=null, bool isRealtime=false, bool isRepeating=false) =>
			new Timer(duration, onComplete, onUpdate, isRealtime, isRepeating, true);


		public void Start() =>
			Restart();

		public void Restart(bool pauseAfterRestart=false) {
			remaining = duration;
			isPaused = pauseAfterRestart;
		}

		public void Cancel() =>
			Restart(true);

		public void Pause() =>
			isPaused = true;

		public void UnPause() =>
			isPaused = false;

		public void SetDuration(float duration) =>
			this.duration = duration;

		public void Tick(float deltaTime, float unscaledDeltaTime) {
			if (!isComplete && !isPaused) {
				remaining -= isRealtime ? unscaledDeltaTime : deltaTime;
				onUpdate?.Invoke(remaining);
			}
			// Check for interval (ms, s, min) triggers
			if (onMillisecond != null && prevMillisecondFired - remaining > 0.1f) {
				onMillisecond?.Invoke();
				prevMillisecondFired = remaining;
			}
			if (onSecond != null && prevSecondFired - remaining > 1) {
				onSecond?.Invoke();
				prevSecondFired = remaining;
			}
			if (onMinute != null && prevMinuteFired - remaining > 60) {
				onMinute?.Invoke();
				prevMinuteFired = remaining;
			}

			// Check for complete
			if (!isComplete && remaining <= 0) {
				onComplete?.Invoke();
				isComplete = true;

				if (isRepeating) {
					remaining += duration;
					completeTime += duration;
					isComplete = false;
				}
				
				if (!isPersistent)
					Dispose();
			}
		}

		public void Dispose() {
			onComplete = null;
			onUpdate = null;
			if (TimerManager.Instance != null)
				TimerManager.Instance?.RemoveTimer(this);
		}
	#endregion Methods
	}

}