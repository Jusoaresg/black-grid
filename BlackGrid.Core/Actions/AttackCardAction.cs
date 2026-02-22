using BlackGrid.Core.Turn;

namespace BlackGrid.Core.Actions;

public record AttackCardAction(int PlayerId, int SelectedColumn) : IGameAction
{
	public int PlayerId { get; } = PlayerId;
	public int SelectedColumn { get; } = SelectedColumn;

	public bool CanExecute(GameState state)
	{
		return state.Phase == Phase.DeclareAttack;
	}

	public void Execute(GameState state)
	{
		if (state.Phase != Phase.DeclareAttack)
			return;

		var player = state.ActualPlayer;

		if (SelectedColumn < 0 || SelectedColumn >= player.Board.Columns.Length)
			return;

		var column = player.Board.Columns[SelectedColumn];
		if (column == null)
			return;

		column.SetWillAttack(!column.WillAttack);
	}
}
