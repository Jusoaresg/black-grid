namespace BlackGrid.Core.Turn;

public static class TurnResolver
{
	public static void ResolveStartPhase(GameState state)
	{
		var player = state.ActualPlayer;

		if (player.Deck.Count == 0)
		{
			throw new Exception("Player deck without cards");
		}

		if (state.Turn == 1 && state.ActualPlayerIndex == 0)
		{
			var opponentPlayer = state.OpponentPlayer;
			for (int i = 0; i < 3; i++)
			{
				player.Hand.Add(player.Deck.Draw());
				opponentPlayer.Hand.Add(opponentPlayer.Deck.Draw());
			}
			return;
		}

		player.Hand.Add(player.Deck.Draw());
	}

	public static void ResolveEndTurn(GameState state)
	{
		state.ActualPlayerIndex = 1 - state.ActualPlayerIndex;
		state.Turn++;
	}
}
