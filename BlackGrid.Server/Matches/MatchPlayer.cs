using BlackGrid.Server.Session;

namespace BlackGrid.Server.Matches;

public class MatchPlayer(PlayerSession session, int playerId)
{
	public Guid UserId { get; } = session.UserId;
	public PlayerSession Session { get; } = session;
	public int PlayerId { get; } = playerId;
}
