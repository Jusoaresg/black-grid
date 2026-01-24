using BlackGrid.Core.Actions;
using BlackGrid.Core.Combat;

namespace BlackGrid.Core.Turn;

public class TurnManager(GameState state)
{
	private readonly GameState state = state;

	public void ResolvePhases()
	{
		switch (state.Phase)
		{
			case Phase.StartTurn:
				ResolveStartPhase();
				state.Phase = Phase.Action;
				break;

			case Phase.Action:
				// Wait player input to change phase
				break;

			case Phase.DeclareAttack:
				// Wait player input to change phase
				break;

			case Phase.ResolveCombat:
				CombatSystem.ResolveCombat(state);
				state.Phase = Phase.EndTurn;
				break;

			case Phase.EndTurn:
				ResolveEndTurn();
				ChangePlayer();
				state.Phase = Phase.StartTurn;
				break;
		}
	}

	public void HandleAction(IGameAction action)
	{
		if (action.PlayerId != state.ActualPlayerIndex)
			return;

		action.Execute(state);
	}

	private void ResolveStartPhase()
	{
		var player = state.ActualPlayer;

		if (player.Deck.Count == 0)
			return;

		player.Hand.Add(player.Deck.Draw());
	}

	private void ResolveEndTurn() { }

	private void ChangePlayer()
	{
		state.ActualPlayerIndex = 1 - state.ActualPlayerIndex;
	}
}
