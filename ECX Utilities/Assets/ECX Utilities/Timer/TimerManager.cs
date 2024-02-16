/*
ECX UTILITY SCRIPTS
Timer Manager (Singleton)
Last updated: Feb 15, 2024
*/

using System.Collections.Generic;
using UnityEngine;

namespace EcxUtilities {
	public class TimerManager : Singleton<TimerManager> {
		public List<Timer> timerList { get; private set; } = new List<Timer>();
		private Queue<Timer> toDestroy = new Queue<Timer>();


		private void Update() {
			// Tick clocks
			foreach (Timer timer in timerList)
				timer.Tick(Time.deltaTime);
			
			// Handle removing timers after Ticks are complete so it doesn't modify List and throw an error.
			while (toDestroy.Count > 0)
				timerList.Remove(toDestroy.Dequeue());
		}

		public void AddTimer(Timer timer) =>
			timerList.Add(timer);

		public void RemoveTimer(Timer timer) {
			if (!timerList.Contains(timer))	return;
			
			toDestroy.Enqueue(timer);
		}
	}
}