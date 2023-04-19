using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursuitPlayer : EnemyState
{
	EnemyMove move;
	EnemyAttack attack;

	public override void Awake()
	{
		base.Awake();
		attack = GetComponentInParent<EnemyAttack>();
		move = GetComponentInParent<EnemyMove>();
	}

	public override bool isValid()
	{
		return Vector3.Distance(enemyObject.transform.position,
			PlayerObject.player.transform.position) > attack.range;
	}

	public void Pursuit()
	{
		move.agent.SetDestination(PlayerObject.player.transform.position);
	}

	public void Stop()
	{
		move.agent.ResetPath();
	}
}
