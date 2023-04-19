using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.EventHub;

public class PlayerAttack : MonoBehaviour
{
	PlayerObject playerObject;
	IWeapon weapon;
	public bool weaponAttached { get { return weapon != null; } }
	int triggerLimit = 6;

	private void Awake()
	{
		playerObject = GetComponent<PlayerObject>();
	}

	void Start()
	{
		playerObject.FrameAction += TriggerAttack;
		playerObject.FrameAction += ThrowAttack;
	}

	public void SetWeapon(IWeapon newWeapon)
	{
		if (!weaponAttached)
		{ weapon = newWeapon; }
	}

	void TriggerAttack()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			if (weaponAttached)
			{
				StartCoroutine(Acted());
				if (triggerLimit <= 0)
				{ 
					ThrowWeapon(); 
					triggerLimit = 6;
				}
				else
				{
					weapon.Use();
					triggerLimit--;
				}
			}
		}
	}

	void ThrowWeapon()
	{
		if (weaponAttached)
		{
			StartCoroutine(Acted());
			Vector3 direction = CameraController.instance.transform.forward;
			direction.y += 0.15f;
			weapon.playerThrow = true;
			weapon.Throw(direction.normalized, 15f);
			weapon = null;
		}
	}

	void ThrowAttack()
	{
		if (Input.GetButtonDown("Fire2"))
		{
			ThrowWeapon();
		}
	}

	IEnumerator Acted()
	{
		playerObject.instantAction = true;
		yield return new WaitForSecondsRealtime(playerObject.instantInterval);
		playerObject.instantAction = false;
	}

	void OnDisable()
	{
		playerObject.FrameAction -= TriggerAttack;
		playerObject.FrameAction -= ThrowAttack;
	}
}
