using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.EasingEquations;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Sword : MonoBehaviour, IWeapon
{
	Rigidbody body;
	Collider swordCollider;
	int originalLayer;

	public bool playerThrow { get; set; }
	public int weaponLayer { get { return gameObject.layer; } set { gameObject.layer = value; } }
	public string weaponName { get { return gameObject.name; } }
	public Transform weaponTransform { get { return transform; } }

	void Awake()
	{
		swordCollider = GetComponent<Collider>();
		body = GetComponent<Rigidbody>();
		originalLayer = gameObject.layer;
	}

	public void Use()
	{
		print("sword attack");
	}

	public void Catch(Transform parent)
	{
		swordCollider.enabled = false;
		body.useGravity = false;
		body.velocity = Vector3.zero;
		body.angularVelocity = Vector3.zero;
		gameObject.layer = parent.gameObject.layer;
		StartCoroutine(CatchTween(parent));
	}

	public void CatchEnemy(Transform parent) { }

	IEnumerator CatchTween(Transform parent)
	{
		Vector3 start = transform.position;
		Vector3 startAngle = transform.eulerAngles;
		float t = 0;
		while (t < 1.01f)
		{
			transform.position = EasingVector3Equations.EaseInExpo(start, parent.position, t);
			transform.eulerAngles = EasingVector3Equations.Linear(startAngle, parent.eulerAngles, t);
			t += Time.unscaledDeltaTime * 3f;
			yield return null;
		}

		transform.parent = parent;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
	}

	public void Throw(Vector3 direction, float intensity)
	{
		transform.parent = null;
		swordCollider.enabled = true;
		body.AddForce(direction * intensity, ForceMode.VelocityChange);
		body.AddTorque((transform.right + transform.up) * intensity, ForceMode.VelocityChange);
		body.useGravity = true;
		gameObject.layer = originalLayer;
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.CompareTag("Enemy"))
		{ col.gameObject.GetComponent<EnemyObject>().DeActivate(); }
		Destroy(gameObject);
	}
}
