using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Pool;

public class EnemyAttack : MonoBehaviour
{
	[SerializeField] Transform rightHandPosition;
	public Transform weaponPosition { get { return rightHandPosition; } }

	IWeapon weapon;
	[SerializeField] Gun gunPrefab;
	ObjectPooler<Gun> gunPool;

	public float range;
	public bool weaponAttached { get { return weapon != null; } }

	private void Awake()
	{
		gunPool = new ObjectPooler<Gun>(gunPrefab, 5, 10, Vector2.right * 5000);
	}

	private void Start()
	{
		//weapon = GetComponentInChildren<IWeapon>();
		//weapon.weaponLayer = gameObject.layer;
	}

	public void OnActivate()
	{
		print("asd");
		SetWeapon(gunPool.GetObject());
		weapon.CatchEnemy(weaponPosition);
		//weapon.weaponTransform.GetComponent<Gun>().Activate(-1);
	}

	public void SetWeapon(IWeapon newWeapon)
	{
		if (!weaponAttached)
		{ weapon = newWeapon; }
	}

	public void Attack()
	{
		if (weaponAttached)
		{ weapon.Use(); }
	}

	public void ThrowWeapon()
	{
		if (weaponAttached)
		{
			Vector3 direction = transform.forward;
			direction.y += 0.15f;
			weapon.Throw(direction.normalized, 15f);
			weapon = null;
		}
	}
}
