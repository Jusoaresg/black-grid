using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using BlackGrid.Data;
using BlackGrid.Server.Mapper;
using BlackGrid.Server.Matches;
using BlackGrid.Server.Session;

namespace BlackGrid.Server.Websockets;

public record MatchStartDto(Guid MatchId, int PlayerId);
public record GameOverDto(Guid MatchId, int WinnerPlayerId, string Reason);

public class MatchNotifier
{
	public static async Task BroadcastMatchStart(Match match)
	{
		await SendMatchStart(match, match.PlayerA.Session);
		await SendMatchStart(match, match.PlayerB.Session);
	}

	private static async Task SendMatchStart(Match match, PlayerSession session)
	{
		if (session.Socket.State != WebSocketState.Open)
			return;

		var dto = new MatchStartDto(match.MatchId, match.GetPlayerIndex(session.UserId));

		var json = JsonSerializer.Serialize(new
		{
			type = "match_start",
			payload = dto
		}, JsonOptions.Options);
		var bytes = Encoding.UTF8.GetBytes(json);

		await session.Socket.SendAsync(bytes,
				WebSocketMessageType.Text,
				true,
				CancellationToken.None);
	}

	public static async Task BroadcastGameState(Match match)
	{
		foreach (var player in new[] { match.PlayerA, match.PlayerB })
		{
			var dto = GameStateMapper.ToDto(match.State, match, player.PlayerId);

			var json = JsonSerializer.Serialize(new
			{
				type = "game_state",
				payload = dto
			});

			var bytes = Encoding.UTF8.GetBytes(json);

			await Send(player.Session, bytes);
		}
	}

	public static async Task BroadcastGameOver(Match match, string reason)
	{
		int winnerPlayerIndex = (int)match.State.WinnerPlayerIndex;
		var dto = new
		{
			type = "game_over",
			payload = new GameOverDto(match.MatchId, (int)match.State.WinnerPlayerIndex, reason)
		};
		var json = JsonSerializer.Serialize(dto, JsonOptions.Options);
		var bytes = Encoding.UTF8.GetBytes(json);

		var winnerSession = match.GetPlayerByIndex(winnerPlayerIndex).Session;
		await Send(winnerSession, bytes);
	}

	public static async Task Send(PlayerSession session, byte[] bytes)
	{
		if (session.Socket.State != WebSocketState.Open)
			return;

		await session.Socket.SendAsync(bytes,
				WebSocketMessageType.Text,
				true,
				CancellationToken.None
				);
	}
}
