using UnityEngine;
using EcxUtilities;

public class TimerTest : MonoBehaviour {
	private Timer oneTime, repeating, destructable1, destructable2;

	private void Start() {
		oneTime = new Timer(3, OneTimeDone, null, false, true);
		oneTime.Start();
		repeating = new Timer(3, RepeatingDone, null, true, true);
		repeating.Start();
	}

	private void OnDestroy() {
		oneTime.Destroy();
		repeating.Destroy();
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.D)) {
			destructable1 = new Timer(1, Destructable1Done, null, false, false);
			destructable1.Start();
			// destructable1.onComplete += Destructable1Done;
		}
		if (Input.GetKeyDown(KeyCode.F)) {
			destructable2 = new Timer(1, Destructable2Done, null, false, false);
			destructable2.Start();
		}
		if (Input.GetKeyDown(KeyCode.Backspace)) {
			OnDestroy();
		}

	}

	private void OneTimeDone() =>
		Debug.Log($"OneTime Timer is done!");

	private void RepeatingDone() =>
		Debug.Log($"Repeating Timer is done!");

	private void Destructable1Done() =>
		Debug.Log($"Destructable Timer 1 is done!");

	private void Destructable2Done() =>
		Debug.Log($"Destructable Timer 2 is done!");
}