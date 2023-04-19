using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Utility.EventHub;

public class InfoDisplay : MonoBehaviour
{
	public string eventComplement;
	TextMeshProUGUI textMesh;

	private void Awake()
	{
		textMesh = GetComponent<TextMeshProUGUI>();
	}

	private void Start()
	{
		EventHub.instance.ObserveEvent(EventList.InfoDisplayOn + eventComplement, OnDisplay);
		EventHub.instance.ObserveEvent(EventList.InfoDisplayOff + eventComplement, OffDisplay);
	}

	void OnDisplay(EventData e)
	{
		textMesh.text = (string)e.eventInformation;
	}

	void OffDisplay(EventData e)
	{
		textMesh.text = string.Empty;
	}

	private void OnDestroy()
	{
		EventHub.instance.RemoveObserver(EventList.InfoDisplayOn, OnDisplay);
		EventHub.instance.RemoveObserver(EventList.InfoDisplayOff, OffDisplay);
	}
}
