namespace BlackGrid.Core.Actions;

public record AttackCardAction(int PlayerId, int SelectedColumn) : IGameAction
{
	public int PlayerId { get; } = PlayerId;
	public int SelectedColumn { get; } = SelectedColumn;

	public void Execute(GameState state)
	{
		if (PlayerId != state.ActualPlayerIndex)
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
