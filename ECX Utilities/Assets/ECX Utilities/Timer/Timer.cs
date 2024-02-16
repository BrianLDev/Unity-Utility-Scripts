/*
ECX UTILITY SCRIPTS
Timer
Last updated: Feb 16, 2024
*/

using System;

namespace EcxUtilities {
	public class Timer {
		public float duration { get; private set; }
		public event Action onComplete;
		public event Action<float> onUpdate;
		public bool isRepeating { get; private set; }
		public bool isPersistent { get; private set; }

		public bool isPaused { get; private set; }
		public bool isComplete { get; private set; }
		public float remaining { get; private set; }
		public float elapsed => duration - remaining;
		public float fractionRemaining => remaining / duration;
		public float fractionElapsed => elapsed / duration;

		private Timer(float duration, Action onComplete, Action<float> onUpdate = null, bool isRepeating = false, bool isPersistent = false) {
			this.duration = duration;
			this.onComplete = onComplete;
			this.onUpdate = onUpdate;
			this.isRepeating = isRepeating;
			this.isPersistent = isPersistent;
			isPaused = false;
			isComplete = false;
			remaining = duration;
			TimerManager.Instance.AddTimer(this);
			Start();
		}

		~Timer() =>
			Dispose();

		public static Timer CreateOneShot(float duration, Action onComplete, Action<float> onUpdate = null) =>
			new Timer(duration, onComplete, onUpdate, false, false);

		public static Timer CreatePersistent(float duration, Action onComplete, Action<float> onUpdate = null, bool isRepeating = false) =>
			new Timer(duration, onComplete, onUpdate, isRepeating, true);

		public void Start() =>
			Restart();

		public void Restart() {
			remaining = duration;
			isPaused = false;
		}

		public void Pause() =>
			isPaused = true;

		public void UnPause() =>
			isPaused = false;

		public void SetDuration(float duration) =>
			this.duration = duration;

		public void Tick(float deltaTime) {
			if (!isComplete && !isPaused) {
				remaining -= deltaTime;
				onUpdate?.Invoke(deltaTime);
			}
			
			if (!isComplete && remaining <= 0) {
				onComplete?.Invoke();
				isComplete = true;

				if (isRepeating) {
					remaining += duration;
					isComplete = false;
				}
				else if (!isPersistent)
					Dispose();
			}
		}

		public void Dispose() {
			onComplete = null;
			onUpdate = null;
			if (TimerManager.Instance != null)
				TimerManager.Instance?.RemoveTimer(this);
		}
	}

}