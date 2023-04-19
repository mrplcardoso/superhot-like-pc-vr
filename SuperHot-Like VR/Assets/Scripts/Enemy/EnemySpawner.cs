using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Pool;
using Utility.EventHub;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] EnemyObject[] enemyPrefab;
	ObjectPooler<EnemyObject>[] enemyPool;

	private void Awake()
	{
		enemyPool = new ObjectPooler<EnemyObject>[enemyPrefab.Length];
		for (int i = 0; i < enemyPrefab.Length; i++)
		{
			enemyPool[i] = new ObjectPooler<EnemyObject>(enemyPrefab[i], 10, 100, transform.position, false, false);
		}
		EventHub.instance.ObserveEvent(EventList.EnterUpdateGame, StartSpawn);
	}

	private void StartSpawn(EventData e)
	{
		StartCoroutine(Spawn());
	}

	IEnumerator Spawn()
	{
		while(true)
		{
			int r = Random.Range(0, enemyPool.Length);
			EnemyObject e = enemyPool[r].GetObject();
			e.transform.position = transform.position;
			e.Activate(-1);
			//yield break;
			yield return new WaitForSeconds(10f);
		}
	}

	void OnDestroy()
	{
		EventHub.instance.RemoveObserver(EventList.EnterUpdateGame, StartSpawn);
	}
}
