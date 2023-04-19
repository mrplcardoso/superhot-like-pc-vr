namespace Utility
{
	namespace EventHub
	{
		using EventReaction = EventHub.EventObserverReaction;

		public static class EventHubExtensions
		{
			public static void PostEvent(this object poster, string eventName, EventData args)
			{
				EventHub.instance.PostEvent(eventName, args);
			}
			public static void PostEvent(this object poster, string eventName)
			{
				EventHub.instance.PostEvent(eventName, EventData.empty);
			}

			public static void ObserveEvent(this object observer, string eventName, EventReaction eventReaction)
			{
				EventHub.instance.ObserveEvent(eventName, eventReaction);
			}

			public static void RemoveObserver(this object observer, string eventName, EventReaction eventReaction)
			{
				EventHub.instance.RemoveObserver(eventName, eventReaction);
			}
		}
	}
}