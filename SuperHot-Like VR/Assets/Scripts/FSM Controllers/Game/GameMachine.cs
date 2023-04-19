using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.FSM;

public class GameMachine : AbstractMachine
{
	void Start()
	{
		GameObject g = GameObject.FindGameObjectWithTag("PlayerCanvas");
		Canvas c = (g != null) ? g.GetComponent<Canvas>() : null;
		if(c != null)
		{ c.worldCamera = CameraController.instance.sceneCamera; }
		ChangeStateIntervaled<LoadGame>();
	}
}
