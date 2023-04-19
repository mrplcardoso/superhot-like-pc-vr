using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
	public bool playerThrow { get; set; }
	public int weaponLayer { get; set; }
	public string weaponName { get; }
	public Transform weaponTransform { get; }
	void Use();
	void Catch(Transform parent);
	void CatchEnemy(Transform parent);
	void Throw(Vector3 direction, float intensity);
}
