using Utility.FSM;

public class TitleState : AbstractState
{
	public TitleMachine titleMachine
	{ get { return (TitleMachine)stateMachine; } }
}
