using UnityEngine;
using EcxUtilities;

public class TimerTest : MonoBehaviour {
	[SerializeField] private int spamTimers;
	private Timer oneTime, repeating, destructable1, destructable2;
	private int oneShotCount, repeatingCount, destructable2Count, spamCount;

	private void Start() {
		oneTime = Timer.CreatePersistent(3, OneShotDone);
		repeating = Timer.CreatePersistent(2, () => repeatingCount += 1, null, true);
		repeating.onComplete += RepeatingDone;
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.O))
			oneTime = Timer.CreatePersistent(3, OneShotDone);
		if (Input.GetKeyDown(KeyCode.D))
			destructable1 = Timer.CreateOneShot(1, Destructable1Done);
		if (Input.GetKeyDown(KeyCode.F)) {
			destructable2 = Timer.CreateOneShot(1, () => destructable2Count += 1);
			destructable2.onComplete += Destructable2Done;
		}
		if (Input.GetKeyDown(KeyCode.S)) {
			for (int i=0; i<spamTimers; i++)
				Timer.CreateOneShot(2, () => spamCount += 1);
		}
		if (Input.GetKeyDown(KeyCode.C))
			Debug.Log($"Current spamTimers count is {spamCount}");
	}

	private void OneShotDone() {
		oneShotCount += 1;
		Debug.Log($"OneShot Timer is done!  Current count is {oneShotCount}");
	}

	private void RepeatingDone() =>
		Debug.Log($"Repeating Timer is done!  Current count is {repeatingCount}");

	private void Destructable1Done() =>
		Debug.Log($"Destructable Timer 1 is done!");

	private void Destructable2Done() =>
		Debug.Log($"Destructable Timer 2 is done!  Current count is {destructable2Count}");
	
}