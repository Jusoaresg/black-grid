using BlackGrid.Core.Turn;

namespace BlackGrid.Core.Actions;

public class PlayCardAction(int playerId, string cardInstanceId, int targetColumn) : IGameAction
{
	public int PlayerId { get; } = playerId;
	public string CardInstanceId { get; } = cardInstanceId;
	public int TargetColumn { get; } = targetColumn;

	public bool CanExecute(GameState state)
	{
		return state.Phase == Phase.Action;
	}

	public void Execute(GameState state)
	{
		if (state.Phase != Phase.Action)
			return;

		var player = state.ActualPlayer;
		var card = player.Hand.FirstOrDefault(c => c.InstanceId == CardInstanceId);

		if (card == null)
			return;

		var column = player.Board.Columns[TargetColumn];
		if (!column.CanPlace(card))
			return;

		player.Hand.Remove(card);
		column.Place(card);
	}
}
