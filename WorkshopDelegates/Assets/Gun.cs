using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gun : MonoBehaviour {

	public GameObject bulletPrefab;

	event UnityAction onFire = delegate { };

	public float fireSpeed = 4;
	public float reloadSpeed = .35f;
	bool reloaded = true;


	void Start() {
		onFire += ShootBullet;

		onFire += () => { StartCoroutine(Reload()); };

		MuzzleFlash flash = this.GetComponentInChildren<MuzzleFlash>();
		if (flash)
			onFire += flash.Flash;

		Animator anim = this.GetComponent<Animator>();
		if (anim)
			onFire += () => { anim.SetTrigger("Fire"); };

		CameraRumble rumble = FindObjectOfType<CameraRumble>();
		if (rumble)
			onFire += rumble.Rumble;

		GameManager gameManager = FindObjectOfType<GameManager>();
		if (gameManager)
			onFire += gameManager.AddPoint;
	}

	public void Fire() {
		if (reloaded)
			onFire();
	}

	void ShootBullet() {
		GameObject bullet = Instantiate(bulletPrefab, this.transform.position + Vector3.right, Quaternion.identity);
		bullet.GetComponent<Rigidbody>().velocity = Vector3.right * fireSpeed;
		Destroy(bullet, 5);
	}

	IEnumerator Reload() {
		reloaded = false;
		yield return new WaitForSeconds(reloadSpeed);
		reloaded = true;
	}

}
