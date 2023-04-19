using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility.EventHub;
using Utility.Audio;

public class StartTitle : TitleState
{
	public override void OnEnter()
	{
		EventHub.instance.ObserveEvent(EventList.BlackScreenEnd, GameScene);
		EventHub.instance.PostEvent(EventList.EnterStartTitle);
		AudioHub.instance.PlayLoop(AudioList.Background);
		StartCoroutine(WaitInput());
	}

	IEnumerator WaitInput()
	{
		yield return new WaitWhile(() => !Input.GetButtonDown("Fire1"));
		EventHub.instance.PostEvent(EventList.BlackScreenOn);
	}

	void GameScene(EventData e)
	{
		EventHub.instance.RemoveObserver(EventList.BlackScreenEnd, GameScene);
		SceneManager.LoadScene("Map");
	}

	public override void OnExit()
	{
		EventHub.instance.PostEvent(EventList.ExitStartTitle);
		//EventHub.instance.RemoveObserver(EventList.BlackScreenEnd, NextState);
	}
}
