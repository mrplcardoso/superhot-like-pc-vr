using System.Collections.Generic;
using Utility.EventHub;
using Utility.ActionPhase;

public class LoadGame : GameState
{
	List<IInitialize> initializers;

	private void Awake()
	{
		initializers = new List<IInitialize>();
		EventHub.instance.ObserveEvent(EventList.AddInitializer, AddInitializer);
		EventHub.instance.ObserveEvent(EventList.BlackScreenEnd, NextState);
	}

	void AddInitializer(EventData e)
	{
		IInitialize initialize = (IInitialize)e.eventInformation;
		if (!initializers.Contains(initialize))
		{ initializers.Add(initialize); }
	}

	public override void OnEnter()
	{
		EventHub.instance.PostEvent(EventList.EnterLoadGame);
		EventHub.instance.PostEvent(EventList.BlackScreenOff);
		gameMachine.ChangeStateIntervaled<StartGame>(1f);
	}

	void NextState(EventData e)
	{
		gameMachine.ChangeState<StartTitle>();
	}

	public override void OnExit()
	{
		for(int i = 0; i < initializers.Count; i++)
		{ initializers[i].Initialize(); }
		EventHub.instance.PostEvent(EventList.ExitLoadGame);
		EventHub.instance.RemoveObserver(EventList.BlackScreenEnd, NextState);
	}

	private void OnDestroy()
	{
		EventHub.instance.RemoveObserver(EventList.AddInitializer, AddInitializer);
	}
}
