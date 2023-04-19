using System.Collections.Generic;
using Utility.EventHub;
using Utility.ActionPhase;

public class UpdateGame : GameState
{
	List<IFrameUpdate> frameList;
	List<IPhysicsUpdate> physicsList;
	List<IPostUpdate> postList;

	public bool doUpdate { get; private set; }

	private void Awake()
	{
		doUpdate = false;
		frameList = new List<IFrameUpdate>();
		EventHub.instance.ObserveEvent(EventList.AddFrameUpdater, AddFrameUpdater);
		physicsList = new List<IPhysicsUpdate>();
		EventHub.instance.ObserveEvent(EventList.AddPhysicsUpdater, AddPhysicsUpdater);
		postList = new List<IPostUpdate>();
		EventHub.instance.ObserveEvent(EventList.AddPostUpdater, AddPostUpdater);
		EventHub.instance.ObserveEvent(EventList.PlayerDeath, OnPlayerKilled);
	}

	void AddFrameUpdater(EventData e)
	{
		IFrameUpdate frame = (IFrameUpdate)e.eventInformation;
		if (!frameList.Contains(frame))
		{ frameList.Add(frame); }
	}

	void AddPhysicsUpdater(EventData e)
	{
		IPhysicsUpdate physics = (IPhysicsUpdate)e.eventInformation;
		if (!physicsList.Contains(physics))
		{ physicsList.Add(physics); }
	}

	void AddPostUpdater(EventData e)
	{
		IPostUpdate post = (IPostUpdate)e.eventInformation;
		if (!postList.Contains(post))
		{ postList.Add(post); }
	}

	public override void OnEnter()
	{
		EventHub.instance.PostEvent(EventList.EnterUpdateGame);
		doUpdate = true;
		CameraController.instance.driver.enabled = true;
	}

	private void Update()
	{
		if (!doUpdate) { return; }

		for (int i = 0; i < frameList.Count; i++)
		{ if (frameList[i] != null) { if (frameList[i].isActive) { frameList[i].FrameUpdate(); } } }
	}

	private void FixedUpdate()
	{
		if (!doUpdate) { return; }

		for (int i = 0; i < physicsList.Count; i++)
		{ if (physicsList[i] != null) { if (physicsList[i].isActive) { physicsList[i].PhysicsUpdate(); } } }
	}

	private void LateUpdate()
	{
		if (!doUpdate) { return; }

		for (int i = 0; i < postList.Count; i++)
		{ if (postList[i] != null) { if (postList[i].isActive) { postList[i].PostUpdate(); } } }
	}

	void OnPlayerKilled(EventData e)
	{
		TimeScaler.SetTimeScale(1f);
		doUpdate = false;
		gameMachine.ChangeState<RestartGame>();
	}

	private void OnDestroy()
	{
		EventHub.instance.RemoveObserver(EventList.AddFrameUpdater, AddFrameUpdater);
		EventHub.instance.RemoveObserver(EventList.AddPhysicsUpdater, AddPhysicsUpdater);
		EventHub.instance.RemoveObserver(EventList.AddPostUpdater, AddPostUpdater);
		EventHub.instance.RemoveObserver(EventList.PlayerDeath, OnPlayerKilled);
	}
}
