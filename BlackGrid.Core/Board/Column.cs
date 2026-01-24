using BlackGrid.Core.Cards;

namespace BlackGrid.Core.Board;

//NOTE: Remove these defaults later. Will generate phantom bugs
public class Column
{
	public Slot Front { get; } = new();
	public Slot Back { get; } = new();
	public bool WillAttack { get; private set; }

	public bool CanPlace(CardInstance card)
	{
		return card.CardDefinition.Type switch
		{
			CardType.Entity => Front.IsEmpty,
			_ => Back.IsEmpty
		};
	}

	public void Place(CardInstance card)
	{
		if (!CanPlace(card))
			throw new InvalidOperationException($"Cannot place {card.CardDefinition.Type} in this column");

		switch (card.CardDefinition.Type)
		{
			case CardType.Entity:
				Front.Place(card);
				break;
			default:
				Back.Place(card);
				break;
		}
	}

	public void SetWillAttack(bool willAttack)
		=> WillAttack = willAttack;
}
