using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : EnemyState
{
	EnemyAttack attack;
	LayerMask mask;
	readonly float time = 2f;
	float interval;

	public override void Awake()
	{
		base.Awake();
		mask = LayerMask.NameToLayer("Player");
		attack = GetComponentInParent<EnemyAttack>();
	}

	public override bool isValid()
	{
		bool inDistance = Vector3.Distance(enemyObject.transform.position,
		PlayerObject.player.transform.position) < attack.range;

		Vector3 d = (PlayerObject.player.transform.position -
			enemyObject.transform.position).normalized;
		bool inSight = Physics.Raycast(enemyObject.transform.position, d, attack.range + 5, mask);

		return inDistance && inSight;
	}

	public void Attack()
	{
		if (interval <= 0)
		{
			interval = time;
			attack.Attack();
		}
		interval -= Time.deltaTime;
	}
}
