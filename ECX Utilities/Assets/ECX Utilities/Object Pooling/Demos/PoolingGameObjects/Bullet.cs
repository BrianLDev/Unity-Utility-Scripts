using UnityEngine;
using EcxUtilities;

public class Bullet : MonoBehaviour {
	private float speed, accel;

	void OnEnable() {
		speed = 0f;
		accel = 0.2f;
	}

	private void FixedUpdate() {
		speed += accel;
		transform.position += Vector3.up * speed * Time.fixedDeltaTime;

		if (transform.position.y > 10)
			Release();
	}

	void Release() =>
		PoolManager.ReleaseObject(this.gameObject);
}
