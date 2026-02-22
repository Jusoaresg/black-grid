using BlackGrid.Core.Cards;

namespace BlackGrid.Core.Board;

public class Slot
{
	public CardInstance? Card { get; set; }
	public bool IsEmpty => Card == null;

	internal void Place(CardInstance card)
	{
		// card.IsExhausted = true;

		card.IsRevealed = card.CardDefinition.Type switch
		{
			CardType.Entity => true,
			_ => false,
		};

		Card = card;
	}

	internal void Clear()
	{
		Card = null;
	}
}

