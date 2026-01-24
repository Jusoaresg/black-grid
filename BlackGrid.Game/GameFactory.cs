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
		var cardDb = new CardDatabase("BlackGrid.Data/Cards");
		var deckDb = new DeckDatabase("BlackGrid.Data/Decks", cardDb);

		var deckDef = deckDb.Get("DECK_CORE_PRESSURE");
		var instances = deckDef.CardIds.Select(id => new CardInstance(Guid.NewGuid().ToString(), cardDb.Get(id)));
		instances.Shuffle();
		var deck = new Deck(instances);

		//NOTE: Change player creation
		Player player1 = new Player(0, deck, new PlayerBoard());
		Player player2 = new Player(1, deck, new PlayerBoard());

		return new GameState(config, new Player[] { player1, player2 });
	}
}
