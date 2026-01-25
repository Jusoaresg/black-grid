using System.Text;
using System.Text.Json;
using BlackGrid.Server.Matchmaking;
using BlackGrid.Server.Session;

namespace BlackGrid.Server.Websockets;

public class MessageHandler
{
	public static async Task HandleMesssage(PlayerSession session, string json)
	{
		var doc = JsonDocument.Parse(json);
		var type = doc.RootElement.GetProperty("type").GetString();

		switch (type)
		{
			case "find_match":
				if (session.CurrentMatch == null)
					await HandleFindMatch(session);
				break;
			case "play_card":
				break;
			case "end_phase":
				break;
		}
	}

	private static async Task HandleFindMatch(PlayerSession session)
	{
		var match = MatchmakingService.Instance.TryCreateMatch(session);

		if (match is null)
		{
			var message = new
			{
				type = "matchmaking",
				payload = new
				{
					status = "waiting"
				}
			};
			var json = JsonSerializer.Serialize(message);
			var bytes = Encoding.UTF8.GetBytes(json);
			await MatchNotifier.Send(session, bytes);
		}
		else
		{
			match.TurnManager.ResolvePhases();
			await MatchNotifier.BroadcastMatchStart(match);
			await MatchNotifier.BroadcastGameState(match);
		}
	}
}
