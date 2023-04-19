using System.Collections.Generic;
using UnityEngine;
using Utility.EventHub;
using Utility.ActionPhase;

public class LoadTitle : TitleState
{
	private void Awake()
	{
		EventHub.instance.ObserveEvent(EventList.BlackScreenEnd, NextState);
	}

	public override void OnEnter()
	{
		CameraController.instance.driver.enabled = false;
		CameraController.instance.transform.rotation = Quaternion.identity;
		EventHub.instance.PostEvent(EventList.EnterLoadTitle);
		EventHub.instance.PostEvent(EventList.BlackScreenOff);
	}

	void NextState(EventData e)
	{
		titleMachine.ChangeState<StartTitle>();
	}

	public override void OnExit()
	{
		EventHub.instance.PostEvent(EventList.ExitLoadTitle);
		EventHub.instance.RemoveObserver(EventList.BlackScreenEnd, NextState);
	}
}
