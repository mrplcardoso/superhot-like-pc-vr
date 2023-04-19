using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.EventHub;

public static class KillCounter
{
	public static int kills { get; private set; }

	public static void SetKills(int addValue)
	{
		kills += addValue;
		EventHub.instance.PostEvent(EventList.InfoDisplayOn + "kills", new EventData(kills.ToString()));
	}
}
