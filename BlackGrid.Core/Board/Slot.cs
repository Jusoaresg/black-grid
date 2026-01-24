using BlackGrid.Core.Cards;

namespace BlackGrid.Core.Board;

public class Slot
{
	public CardInstance? Card { get; set; }
	public bool IsEmpty => Card == null;

	internal void Place(CardInstance card)
	{
		Card = card;
	}

	internal void Clear()
	{
		Card = null;
	}
}

