using BlackGrid.Core.Players;
using BlackGrid.Core.Turn;

namespace BlackGrid.Core;

public class GameState(GameConfig config, Player[] players)
{
	public GameConfig Config = config;

	public Player[] Players = players;
	public int ActualPlayerIndex = 0;
	public Player ActualPlayer => Players[ActualPlayerIndex];
	public Player OpponentPlayer => Players[1 - ActualPlayerIndex];

	public Phase Phase = Phase.StartTurn;
	public int Turn = 1;

	public bool GameOver;
	public int? WinnerPlayerIndex;
}
