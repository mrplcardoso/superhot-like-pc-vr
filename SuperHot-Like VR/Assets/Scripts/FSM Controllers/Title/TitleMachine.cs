using Utility.FSM;

public class TitleMachine : AbstractMachine
{
	void Start()
	{
		ChangeStateIntervaled<LoadTitle>(0.01f);
	}
}
