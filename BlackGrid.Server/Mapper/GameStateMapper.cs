using BlackGrid.Core;
using BlackGrid.Core.Cards;
using BlackGrid.Server.Dtos;
using BlackGrid.Server.Matches;

namespace BlackGrid.Server.Mapper;

public static class GameStateMapper
{
	public static GameStateDto ToDto(GameState state, Match match, int viewerPlayerId)
	{
		return new GameStateDto(
			match.MatchId,
			state.ActualPlayerIndex,
			state.Players.Select(p =>
			{
				var hand = p.Id == viewerPlayerId ? p.Hand.ToList() : null;
				return new PlayerDto(
					p.Id,
					p.Life,
					p.Board.Columns.Select(c => new ColumnDto(
						ToCardDto(c.Front.Card),
						ToCardDto(c.Back.Card),
						c.WillAttack
					)).ToArray(),
					p.Hand.Count,
					hand
				);
			}).ToArray(),
			(int)state.Phase
		);
	}

	private static CardDto? ToCardDto(CardInstance? card)
	{
		if (card == null) return null;

		return new CardDto(
			card.InstanceId,
			card.IsRevealed,
			new CardDefinitionDto(
				card.CardDefinition.Id,
				card.CardDefinition.Name,
				card.CardDefinition.Description,
				card.CardDefinition.Attack
			)
		);
	}
}
