using System.Net.WebSockets;
using BlackGrid.Server.Matches;

namespace BlackGrid.Server.Session;

public class PlayerSession(WebSocket webSocket)
{
	public Guid SessionId { get; } = Guid.NewGuid();
	public Guid UserId { get; } = Guid.NewGuid();

	public WebSocket Socket { get; } = webSocket;
	public Match? CurrentMatch { get; private set; }

	public void SetCurrentMatch(Match match)
		=> CurrentMatch = match;

	public void ClearMatch()
	{
		CurrentMatch = null;
	}
}
