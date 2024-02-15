using UnityEngine;
using EcxUtilities;

public class Gun : MonoBehaviour {
	[SerializeField] private GameObject bulletPrefab;
	private Vector3 pos;

	//Optional: Warm the pool and preallocate memory
	void Start() {
		bulletPrefab.SetActive(false);
		PoolManager.CreatePool(bulletPrefab, 50);
	}

	void Update() {
		if (Input.GetButton("Fire1")) {
			pos.x = Input.mousePosition.x;
			pos.y = Input.mousePosition.y;
			pos.z = 10f;
			FireBullet(Camera.main.ScreenToWorldPoint(pos), Quaternion.identity);
		}
	}

	void FireBullet(Vector3 position, Quaternion rotation) =>
		PoolManager.SpawnObject(bulletPrefab, position, rotation).GetComponent<Bullet>();

}
