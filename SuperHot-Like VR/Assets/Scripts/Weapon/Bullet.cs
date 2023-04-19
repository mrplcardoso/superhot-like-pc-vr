using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Pool;
using Utility.ActionPhase;
using Utility.EventHub;

public class Bullet : MonoBehaviour, IPoolableObject, IPhysicsUpdate
{
	#region Pool
	public int poolIndex { get { return index; } set { if(index < 0) index = value; } }
	int index = -1;

	public float leftDuration { get { return duration; } }
	float duration = 0;

	public bool stopTimer { get { return duration <= 0; } set { if (value) duration = 0; } }

	public bool activeInScene { get { return gameObject.activeInHierarchy; } }

	public void Activate(float duration)
	{
		this.duration = duration;
		trail.Clear();
		gameObject.SetActive(true);
		StartCoroutine(Timer());
	}

	IEnumerator Timer()
	{
		while(duration > 0)
		{
			duration -= Time.deltaTime;
			yield return null;
		}
		DeActivate();
	}

	public void DeActivate()
	{
		gameObject.SetActive(false);
	}
	#endregion Pool

	#region Update
	TrailRenderer trail;
	Rigidbody body;
	float speed = 5f;
	public bool isActive { get { return activeInScene; } }

	void Awake()
	{
		trail = GetComponent<TrailRenderer>();
		body = GetComponent<Rigidbody>();
		EventHub.instance.ObserveEvent(EventList.EnterUpdateGame, OnUpdateGame);
	}

	public void PhysicsUpdate()
	{
		body.MovePosition(body.position + transform.forward * speed * Time.deltaTime);
	}

	void OnUpdateGame(EventData e)
	{
		EventHub.instance.PostEvent(EventList.AddPhysicsUpdater, new EventData(this));
	}
	#endregion Update

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.CompareTag("Enemy"))
		{ col.gameObject.GetComponent<EnemyObject>().DeActivate(); }
		if (col.gameObject.CompareTag("Player"))
		{ EventHub.instance.PostEvent(EventList.PlayerDeath); }
		DeActivate();
	}

	void OnDestroy()
	{
		EventHub.instance.RemoveObserver(EventList.EnterUpdateGame, OnUpdateGame);
	}
}
