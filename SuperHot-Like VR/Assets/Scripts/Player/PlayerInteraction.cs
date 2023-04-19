using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.EventHub;

public class PlayerInteraction : MonoBehaviour
{
	PlayerObject playerObject;
	PlayerAttack playerAttack;
	[SerializeField] Transform rightHandPosition;
	[SerializeField] LayerMask mask;
	[SerializeField] float range;

	private void Awake()
	{
		playerObject = GetComponent<PlayerObject>();
		playerAttack = GetComponent<PlayerAttack>();
	}

	private void Start()
	{
		rightHandPosition = CameraController.instance.transform.GetChild(0);
		playerObject.FrameAction += TriggerRay;
	}

	void TriggerRay()
	{
		if (!playerAttack.weaponAttached)
		{
			RaycastHit hit = CameraController.instance.raycaster.Raycast(range, mask);
			if (hit.collider != null)
			{
				IWeapon weapon = hit.collider.GetComponent<IWeapon>();
				if (weapon != null && !weapon.playerThrow)
				{  
					EnableDisplay(weapon.weaponName);
					if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.E))
					{
						StartCoroutine(Acted());
						weapon.Catch(rightHandPosition);
						playerAttack.SetWeapon(weapon);
					}
				}
				return;
			}
		}
		DisableDisplay();
	}

	IEnumerator Acted()
	{
		playerObject.instantAction = true;
		yield return new WaitForSecondsRealtime(playerObject.instantInterval);
		playerObject.instantAction = false;
	}

	void EnableDisplay(string weaponName)
	{
		EventHub.instance.PostEvent(EventList.InfoDisplayOn + "player", new EventData(weaponName));
	}

	void DisableDisplay()
	{
		EventHub.instance.PostEvent(EventList.InfoDisplayOff + "player");
	}

	private void OnDisable()
	{
		playerObject.FrameAction -= TriggerRay;
	}
}