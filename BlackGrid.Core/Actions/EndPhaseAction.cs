using BlackGrid.Core.Turn;

namespace BlackGrid.Core.Actions;

public record EndPhaseAction(int PlayerId) : IGameAction
{
	public bool CanExecute(GameState state)
	{
		return state.Phase == Phase.Action || state.Phase == Phase.DeclareAttack;
	}

	public void Execute(GameState state)
	{
		if (PlayerId != state.ActualPlayerIndex)
			return;

		switch (state.Phase)
		{
			case Phase.Action:
				state.Phase = Phase.DeclareAttack;
				break;

			case Phase.DeclareAttack:
				state.Phase = Phase.ResolveCombat;
				break;
		}
	}
}
