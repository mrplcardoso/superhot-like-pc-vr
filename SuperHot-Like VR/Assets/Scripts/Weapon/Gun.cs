using System.Collections;
using UnityEngine;
using Utility.EasingEquations;
using Utility.Pool;
using Utility.Audio;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Gun : MonoBehaviour, IWeapon, IPoolableObject
{
	Rigidbody body;
	Collider gunCollider;
	Cannon cannon;
	int originalLayer;

	public bool playerThrow { get; set; }
	public int weaponLayer { get { return gameObject.layer; } set { gameObject.layer = value; } }
	public string weaponName { get { return gunName; } }
	[SerializeField] string gunName;
	public Transform weaponTransform { get { return transform; } }

	void Awake()
	{
		gunCollider = GetComponent<Collider>();
		body = GetComponent<Rigidbody>();
		cannon = GetComponentInChildren<Cannon>();
		originalLayer = gameObject.layer;
	}

	void OnEnable()
	{
		playerThrow = false;
	}

	public void Use()
	{
		AudioHub.instance.PlayOneTime(AudioList.Shot);
		string l = LayerMask.LayerToName(gameObject.layer);
		cannon.Shoot(LayerMask.NameToLayer(l+"Bullet"));
	}

	public void Catch(Transform parent)
	{
		gunCollider.enabled = false;
		body.useGravity = false;
		body.velocity = Vector3.zero;
		body.angularVelocity =	Vector3.zero;
		gameObject.layer = parent.gameObject.layer;
		StartCoroutine(CatchTween(parent));
	}

	public void CatchEnemy(Transform parent)
	{
		gunCollider.enabled = false;
		body.useGravity = false;
		body.velocity = Vector3.zero;
		body.angularVelocity = Vector3.zero;
		gameObject.layer = parent.gameObject.layer;
		transform.parent = parent;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		Activate(-1);
	}

	IEnumerator CatchTween(Transform parent)
	{
		Vector3 start = transform.position;
		Vector3 startAngle = transform.eulerAngles;
		float t = 0;
		while(t < 1.01f)
		{
			transform.position = EasingVector3Equations.EaseInExpo(start, parent.position, t);
			transform.eulerAngles = EasingVector3Equations.Linear(startAngle, parent.eulerAngles, t);
			t += Time.unscaledDeltaTime * 5f;
			yield return null;
		}

		transform.parent = parent;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
	}

	public void Throw(Vector3 direction, float intensity)
	{
		transform.parent = null;
		gunCollider.enabled = true;
		body.AddForce(direction * intensity, ForceMode.VelocityChange);
		body.AddTorque((transform.right + transform.up) * intensity, ForceMode.VelocityChange);
		body.useGravity = true;
		gameObject.layer = originalLayer;
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.CompareTag("Enemy"))
		{ 
			col.gameObject.GetComponent<EnemyObject>().DeActivate();
		}
	}

	#region Pool
	public int poolIndex { get { return index; } set { if (index < 0) index = value; } }
	int index = -1;

	public float leftDuration { get { return duration; } }
	float duration = 0;

	public bool stopTimer { get { return duration <= 0; } set { if (value) duration = 0; } }

	public bool activeInScene { get { return gameObject.activeInHierarchy; } }

	public void Activate(float duration)
	{
		this.duration = duration;
		gameObject.SetActive(true);
	}

	public void DeActivate()
	{
		gameObject.SetActive(false);
	}
	#endregion Pool
}
