using System.Net.WebSockets;
using BlackGrid.Server.Matches;
using BlackGrid.Server.Matchmaking;
using BlackGrid.Server.Session;

namespace BlackGrid.Server.Websockets;

public class ConnectionHandler
{
	private readonly MatchmakingService _matchmaking = new();

	public async Task HandleConnection(WebSocket socket)
	{
		var session = new PlayerSession(socket);

		var match = _matchmaking.TryCreateMatch(session);
		if (match is not null)
		{
			Match _match = match;
			await MatchNotifier.BroadcastMatchStart(_match);
		}

		await WebsocketReceiver.ReceiveLoop(session);
	}
}
