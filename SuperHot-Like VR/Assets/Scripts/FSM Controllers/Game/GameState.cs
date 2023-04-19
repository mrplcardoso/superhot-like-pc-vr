using Utility.FSM;

public class GameState : AbstractState
{
	public GameMachine gameMachine
	{ get { return (GameMachine)stateMachine; } }
}
