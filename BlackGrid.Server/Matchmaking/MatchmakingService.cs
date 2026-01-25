using BlackGrid.Server.Matches;
using BlackGrid.Server.Session;

namespace BlackGrid.Server.Matchmaking;

public class MatchmakingService
{
	public static MatchmakingService Instance { get; } = new();

	private PlayerSession? waitingPlayer;

	public Match? TryCreateMatch(PlayerSession player)
	{
		if (waitingPlayer == null)
		{
			waitingPlayer = player;
			return null;
		}

		var a = waitingPlayer;
		var b = player;

		waitingPlayer = null;

		return MatchFactory.Create(a, b);
	}

	public void Remove(PlayerSession session)
	{
		if (waitingPlayer == session)
		{
			waitingPlayer = null;
		}
	}

}
