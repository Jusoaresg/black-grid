using BlackGrid.Core.Cards;

namespace BlackGrid.Core.Decks;

public class Deck(IEnumerable<CardInstance> cards)
{
	public readonly Queue<CardInstance> cards = new(cards);
	public int Count => cards.Count;

	public CardInstance Draw()
	{
		if (Count == 0)
			throw new InvalidOperationException($"Deck empty");

		return cards.Dequeue();
	}
}
