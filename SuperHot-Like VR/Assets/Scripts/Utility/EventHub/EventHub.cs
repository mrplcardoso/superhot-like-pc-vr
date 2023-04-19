using System.Collections.Generic;

namespace Utility
{
	namespace EventHub
	{
		/// <summary>
		/// Singleton class to handle Pub/Sub Pattern
		/// </summary>
		public class EventHub
		{
			public delegate void EventObserverReaction(EventData data);
			/// <summary>
			/// Match an event with a list of observers' reaction
			/// </summary>
			Dictionary<string, List<EventObserverReaction>> eventObservers =
				new Dictionary<string, List<EventObserverReaction>>();

			/// <summary>
			/// Singleton instance
			/// </summary>
			public readonly static EventHub instance = new EventHub();
			private EventHub() { }

			public void ObserveEvent(string eventName, EventObserverReaction eventReaction)
			{
				if (eventName == EventList.Empty)
				{ return; }

				if (string.IsNullOrEmpty(eventName))
				{ PrintConsole.Error("Empty event name"); return; }
				if (eventReaction == null)
				{ PrintConsole.Error("Null reaction to '" + eventName + "' event"); return; }

				if (!eventObservers.ContainsKey(eventName))
				{ eventObservers.Add(eventName, new List<EventObserverReaction>()); }
				else if (eventObservers[eventName].Contains(eventReaction))
				{ PrintConsole.Warning("Already observing '" + eventName + "' event"); return; }

				eventObservers[eventName].Add(eventReaction);
			}

			public void PostEvent(string eventName, EventData e)
			{
				if (eventName == EventList.Empty)
				{ return; }

				if (string.IsNullOrEmpty(eventName))
				{ PrintConsole.Warning("Empty event name"); return; }
				if (!eventObservers.ContainsKey(eventName))
				{ PrintConsole.Warning("No observers to react to '" + eventName + "' event"); return; }

				List<EventObserverReaction> l = eventObservers[eventName];
				for (int i = 0; i < l.Count; ++i)
				{ l[i](e); }
			}

			public void RemoveObserver(string eventName, EventObserverReaction eventReaction)
			{
				if (eventName == EventList.Empty)
				{ return; }

				if (string.IsNullOrEmpty(eventName))
				{ PrintConsole.Error("Empty event name"); return; }
				if (eventReaction == null)
				{ PrintConsole.Error("Null reaction to '" + eventName + "' event"); return; }

				if (!eventObservers.ContainsKey(eventName))
				{ PrintConsole.Warning("No '" + eventName + "' event/observers found"); return; }
				if (!eventObservers[eventName].Contains(eventReaction))
				{ PrintConsole.Warning("No reaction to '" + eventName + "' event found"); return; }

				eventObservers[eventName].Remove(eventReaction);
				if (eventObservers[eventName].Count == 0)
				{ RemoveHub(eventName); }
			}

			/// <summary>
			/// Remove an event and it's list of observers
			/// </summary>
			/// <param name="eventName"></param>
			void RemoveHub(string eventName)
			{
				if (eventName == EventList.Empty)
				{ return; }

				if (string.IsNullOrEmpty(eventName))
				{ PrintConsole.Error("Empty event name"); return; }
				if (!eventObservers.ContainsKey(eventName))
				{ PrintConsole.Warning("No '" + eventName + "' event/observers found"); return; }

				eventObservers[eventName].Clear();
				eventObservers.Remove(eventName);
			}

			public int NumberOfObservers(string eventName)
			{
				return eventObservers[eventName].Count;
			}
		}
	}
}