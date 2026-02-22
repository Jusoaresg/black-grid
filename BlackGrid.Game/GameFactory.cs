using BlackGrid.Core;
using BlackGrid.Core.Board;
using BlackGrid.Core.Cards;
using BlackGrid.Core.Decks;
using BlackGrid.Core.Players;
using BlackGrid.Data.Databases;

namespace BlackGrid.Game;

public static class GameFactory
{
	public static GameState Create(GameConfig config)
	{
		var cardDb = new CardDatabase(Path.Combine(config.DataRootPath, "Cards"));
		var deckDb = new DeckDatabase(Path.Combine(config.DataRootPath, "Decks"), cardDb);

		var deckDef = deckDb.Get("DECK_CORE_PRESSURE");
		var instances = deckDef.CardIds.Select(id => new CardInstance(Guid.NewGuid().ToString(), cardDb.Get(id)));
		var deck1 = new Deck(instances.Shuffle());
		var deck2 = new Deck(instances.Shuffle());

		//NOTE: Change player creation
		Player player1 = new(0, deck1, new PlayerBoard());
		Player player2 = new(1, deck2, new PlayerBoard());

		return new GameState(config, [player1, player2]);
	}
}
