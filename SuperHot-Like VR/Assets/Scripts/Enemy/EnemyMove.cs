using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
	public NavMeshAgent agent { get; private set; }

	private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
	}

	public bool ReachedDestination()
	{
		float offset = agent.radius * 2;
		return Vector3.Distance(transform.position, agent.destination) <= offset;
	}
}
