using System;
using System.Collections.Generic;
using UnityEngine;
using Utility.EventHub;
using Utility.ActionPhase;

public class PlayerObject : MonoBehaviour, IInitialize, IFrameUpdate, IPhysicsUpdate, IPostUpdate
{
	public Action InitializeAction, FrameAction, PhysicsAction, PostAction;
	public static PlayerObject player;
	public bool isActive { get { return gameObject.activeInHierarchy; } }
	public readonly float instantInterval = 0.15f;
	public bool moveAction, instantAction;

	void Start()
	{
		player = this;
		EventHub.instance.ObserveEvent(EventList.EnterLoadGame, OnLoadGame);
		EventHub.instance.ObserveEvent(EventList.EnterUpdateGame, OnUpdateGame);
	}

	public void Initialize()
	{
		if (InitializeAction != null)
		{ InitializeAction(); }
	}

	public void FrameUpdate()
	{
		if (instantAction || moveAction)
		{ TimeScaler.SetTimeScale(1f); }
		else
		{ TimeScaler.SetTimeScale(0.2f); }
		if (FrameAction != null)
		{ FrameAction(); }
	}

	public void PhysicsUpdate()
	{
		if (PhysicsAction != null)
		{ PhysicsAction(); }
	}

	public void PostUpdate()
	{
		if (PostAction != null)
		{ PostAction(); }
	}

	void OnLoadGame(EventData e)
	{
		EventHub.instance.PostEvent(EventList.AddInitializer, new EventData(this));
	}

	void OnUpdateGame(EventData e)
	{
		EventHub.instance.PostEvent(EventList.AddFrameUpdater, new EventData(this));
		EventHub.instance.PostEvent(EventList.AddPhysicsUpdater, new EventData(this));
		EventHub.instance.PostEvent(EventList.AddPostUpdater, new EventData(this));
	}
	/*
			if(col.gameObject.CompareTag("Player"))
		{ EventHub.instance.PostEvent(EventList.PlayerDeath); }
	*/
	/*
	public void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if(hit.gameObject.CompareTag("Bullet"))
		{ EventHub.instance.PostEvent(EventList.PlayerDeath); }
	}*/

	void OnDestroy()
	{
		EventHub.instance.RemoveObserver(EventList.EnterLoadGame, OnLoadGame);
		EventHub.instance.RemoveObserver(EventList.EnterUpdateGame, OnUpdateGame);
	}
}
