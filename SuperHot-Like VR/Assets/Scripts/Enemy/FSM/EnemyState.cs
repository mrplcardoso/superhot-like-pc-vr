using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.FSM;

public abstract class EnemyState : AbstractState
{
	protected EnemyObject enemyObject;

	public virtual void Awake()
	{
		enemyObject = GetComponentInParent<EnemyObject>();
	}

	public abstract bool isValid();
}
