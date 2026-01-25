using System.Net.WebSockets;
using BlackGrid.Core;
using BlackGrid.Core.Actions;
using BlackGrid.Core.Turn;
using BlackGrid.Game;
using BlackGrid.Server.Session;
using BlackGrid.Server.Websockets;

namespace BlackGrid.Server.Matches;

public class Match
{
	public Guid MatchId { get; } = Guid.NewGuid();

	public MatchPlayer PlayerA { get; }
	public MatchPlayer PlayerB { get; }

	public GameState State { get; }
	public TurnManager TurnManager { get; }

	public Match(MatchPlayer a, MatchPlayer b, GameConfig config)
	{
		PlayerA = a;
		PlayerB = b;

		State = GameFactory.Create(config);
		TurnManager = new TurnManager(State);
	}

	public async void HandleAction(Guid userId, IGameAction action)
	{
		if (State.ActualPlayerIndex != GetPlayerIndex(userId))
			return;

		TurnManager.HandleAction(action);
		await MatchNotifier.BroadcastGameState(this);
	}

	public int GetPlayerIndex(Guid userId)
	{
		if (PlayerA.UserId == userId) return 0;
		if (PlayerB.UserId == userId) return 1;

		throw new InvalidOperationException("User not in this match");
	}

	public MatchPlayer GetPlayerByIndex(int playerId)
	{
		if (PlayerA.PlayerId == playerId) return PlayerA;
		if (PlayerB.PlayerId == playerId) return PlayerB;
		throw new InvalidOperationException("Can't find player by index");
	}

	public async Task HandleDisconnect(PlayerSession disconnected)
	{
		var loserPlayerId = GetPlayerIndex(disconnected.UserId);
		var winnerId = loserPlayerId == 0 ? 1 : 0;

		State.GameOver = true;
		State.WinnerPlayerIndex = winnerId;

		var winner = GetPlayerByIndex(winnerId);
		if (winner.Session.Socket.State == WebSocketState.Open)
			await MatchNotifier.BroadcastGameOver(this, "Opponent disconnected");

		PlayerA.Session.ClearMatch();
		PlayerB.Session.ClearMatch();
	}
}
