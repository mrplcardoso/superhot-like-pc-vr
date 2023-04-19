using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Pool;

public class Cannon : MonoBehaviour
{
	[SerializeField] Bullet bulletPrefab;
	ObjectPooler<Bullet> bulletPool;

	private void Awake()
	{
		bulletPool = new ObjectPooler<Bullet>(bulletPrefab, 10, 50, Vector3.down * 5000);
	}

	public void Shoot(int layer)
	{
		Bullet nextBullet = bulletPool.GetObject();
		if (nextBullet == null)
		{ PrintConsole.Error("No more bullets"); return; }

		nextBullet.gameObject.layer = layer;
		nextBullet.transform.position = transform.position;
		nextBullet.transform.rotation = transform.rotation;
		nextBullet.Activate(5f);
	}
}
