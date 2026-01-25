using BlackGrid.Core;
using BlackGrid.Server.Infrastructure;
using BlackGrid.Server.Session;

namespace BlackGrid.Server.Matches;

public static class MatchFactory
{
	public static Match Create(PlayerSession a, PlayerSession b)
	{
		var config = new GameConfig(ServerPaths.DataRootPath);

		var playerA = new MatchPlayer(a, 0);
		var playerB = new MatchPlayer(b, 1);

		var match = new Match(playerA, playerB, config);

		a.SetCurrentMatch(match);
		b.SetCurrentMatch(match);

		return match;
	}
}
