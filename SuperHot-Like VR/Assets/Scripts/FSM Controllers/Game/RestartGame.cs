using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility.EventHub;

public class RestartGame : GameState
{
	public override void OnEnter()
	{
		EventHub.instance.ObserveEvent(EventList.BlackScreenEnd, GameScene);
		EventHub.instance.PostEvent(EventList.EnterRestartGame);
		StartCoroutine(Wait());
	}

	IEnumerator Wait()
	{
		yield return new WaitForSeconds(1f);
		EventHub.instance.PostEvent(EventList.BlackScreenOn);
	}

	void GameScene(EventData e)
	{
		EventHub.instance.RemoveObserver(EventList.BlackScreenEnd, GameScene);
		SceneManager.LoadScene("Map");
	}

	public override void OnExit()
	{
		CameraController.instance.transform.rotation = Quaternion.identity;
		EventHub.instance.PostEvent(EventList.ExitRestartGame);
		//EventHub.instance.RemoveObserver(EventList.BlackScreenEnd, NextState);
	}
}
