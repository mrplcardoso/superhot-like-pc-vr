using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInteraction : MonoBehaviour
{
	[SerializeField] LayerMask mask;
	[SerializeField] float range;

	public IWeapon FindWeapon()
	{
		Collider[] cols = Physics.OverlapSphere(transform.position, range, mask);
		if (cols.Length == 0) { return null; }

		return cols[0].GetComponent<IWeapon>();
	}
}
