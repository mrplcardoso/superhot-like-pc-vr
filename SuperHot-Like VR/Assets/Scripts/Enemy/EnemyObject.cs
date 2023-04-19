using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility.EventHub;
using Utility.FSM;
using Utility.Pool;
using Utility.ActionPhase;

public class EnemyObject : MonoBehaviour, IPoolableObject, IFrameUpdate
{
	Animator anim;
	EnemyAttack enemyAttack;
	PursuitPlayer pursuit;
	AttackPlayer attack;
	Wait wait;

	protected void Awake()
	{
		name = gameObject.GetInstanceID().ToString();
		anim = GetComponentInChildren<Animator>();
		enemyAttack	= GetComponent<EnemyAttack>();
		pursuit = GetComponentInChildren<PursuitPlayer>();
		attack = GetComponentInChildren<AttackPlayer>();
		wait = GetComponentInChildren<Wait>();
		EventHub.instance.PostEvent(EventList.AddFrameUpdater, new EventData(this));
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
		if (enemyAttack != null)
		{ enemyAttack.OnActivate(); }
		gameObject.SetActive(true);
	}

	public void DeActivate()
	{
		KillCounter.SetKills(1);
		if (enemyAttack != null)
		{ enemyAttack.ThrowWeapon(); }
		gameObject.SetActive(false);
	}
	#endregion Pool

	#region ActionPhase
	//public GameObject gameObject { get { return gameObject; } }
	public bool isActive { get { return activeInScene; } }
	public void FrameUpdate()
	{
		float distance = Vector3.Distance(transform.position, PlayerObject.player.transform.position);
		
		if(distance >= enemyAttack.range)	{ anim.SetInteger("Speed", 1); pursuit.Pursuit(); } 
		else { anim.SetInteger("Speed", 0); pursuit.Stop(); }

		if(distance <= enemyAttack.range)
		{
			Vector3 targetDirection = (PlayerObject.player.transform.position - transform.position).normalized;
			Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
			if (Quaternion.Angle(transform.rotation, targetRotation) > 3f)
			{
				transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 50f);
			}
			else
			{ attack.Attack(); }
			if(distance <= enemyAttack.range / 3f)
			{ attack.Attack(); }
			anim.SetBool("Shoot", true);
		}
		else
		{ anim.SetBool("Shoot", false); }
	}
	#endregion
}
