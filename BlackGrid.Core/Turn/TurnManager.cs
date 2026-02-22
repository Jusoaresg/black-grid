using BlackGrid.Core.Actions;
using BlackGrid.Core.Combat;

namespace BlackGrid.Core.Turn;

public class TurnManager(GameState state)
{
	private readonly GameState state = state;
	public event Action? PhaseChanged;

	public void ResolvePhases()
	{
		bool resolvingPhases = true;
		while (resolvingPhases)
		{
			resolvingPhases = state.Phase switch
			{
				Phase.StartTurn => ResolveStart(),
				Phase.ResolveCombat => ResolveCombat(),
				Phase.EndTurn => ResolveEnd(),
				_ => false,
			};
		}
	}

	public void HandleAction(IGameAction action)
	{
		//NOTE: Maybe reduntant as Match already checks
		if (action.PlayerId != state.ActualPlayerIndex)
			return;

		if (!action.CanExecute(state))
			return;

		action.Execute(state);

		//NOTE: Change this when player wanted to pass phase. Like more than 1 action per phase
		RequestAdvancePhase(action.PlayerId);
		// ResolvePhases();
	}

	private void AdvancePhase()
	{
		state.Phase = state.Phase switch
		{
			Phase.StartTurn => Phase.Action,
			Phase.Action => Phase.DeclareAttack,
			Phase.DeclareAttack => Phase.ResolveCombat,
			Phase.ResolveCombat => Phase.EndTurn,
			Phase.EndTurn => Phase.StartTurn,
			_ => throw new Exception($"State {state.Phase} does not have a defined transition."),
		};
		PhaseChanged?.Invoke();
	}

	public void RequestAdvancePhase(int playerId)
	{
		if (playerId != state.ActualPlayerIndex)
			return;

		AdvancePhase();
		ResolvePhases();
	}

	private bool ResolveStart()
	{
		TurnResolver.ResolveStartPhase(state);
		AdvancePhase();
		return true;
	}

	private bool ResolveEnd()
	{
		TurnResolver.ResolveEndTurn(state);
		AdvancePhase();
		return true;
	}

	private bool ResolveCombat()
	{
		CombatSystem.ResolveCombat(state);
		AdvancePhase();
		return true;
	}

}
