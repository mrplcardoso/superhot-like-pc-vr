public static class EventList
{
	public const string Empty = "Empty";

	public const string BlackScreenOn = "BlackScreenOn";
	public const string BlackScreenOff = "BlackScreenOff";
	public const string BlackScreenEnd = "BlackScreenEnd";

	public const string OnPlayerDie = "OnPlayerDie";

	/* Title Events */
	#region Title
	public const string EnterLoadTitle = "EnterLoadTitle";
	public const string ExitLoadTitle = "ExitLoadTitle";

	public const string EnterStartTitle = "EnterStartTitle";
	public const string ExitStartTitle = "ExitStartTitle";
	#endregion

	/* Game Events */
	#region Game
	public const string EnterLoadGame = "EnterLoadGame";
	public const string ExitLoadGame = "ExitLoadGame";
	public const string AddInitializer = "AddInitializer";

	public const string EnterStartGame = "EnterStartGame";
	public const string ExitStartGame = "ExitStartGame";

	public const string EnterUpdateGame = "EnterUpdateGame";
	public const string ExitUpdateGame = "ExitUpdateGame";
	public const string AddFrameUpdater = "AddFrameUpdater";
	public const string AddPhysicsUpdater = "AddPhysicsUpdater";
	public const string AddPostUpdater = "AddPostUpdater";

	public const string EnterRestartGame = "EnterRestartGame";
	public const string ExitRestartGame = "ExitRestartGame";

	public const string PlayerDeath = "PlayerDeath";

	public const string InfoDisplayOn = "InfoDisplayOn";
	public const string InfoDisplayOff = "InfoDisplayOff";
	#endregion

	/* Audio Events */
	#region Audio
	public const string AudioPlayOneTime = "AudioPlayOneTime";
	public const string AudioPlayLoop = "AudioPlayLoop";
	public const string AudioPlayIntroLoop = "AudioPlayIntroLoop";
	public const string AudioStop = "AudioStop";
	public const string DestroyAudioHub = "DestroyAudioHub";
	public const string AudioMute = "AudioMute";
	//
	#endregion
}