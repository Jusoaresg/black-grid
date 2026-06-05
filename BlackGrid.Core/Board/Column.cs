using BlackGrid.Core.Cards;

namespace BlackGrid.Core.Board;

//NOTE: Remove these defaults later. Will generate phantom bugs
// Man, i don't remember what defaults ;-;
public class Column
{
	public Slot Front { get; } = new();
	public Slot Back { get; } = new();
	public bool WillAttack { get; private set; }

	public int Tension { get; private set; }
	public bool IsCorrupted
		=> Tension > 3;

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

	public void AddTension(int amount)
	{
		if (amount >= 3)
		{
			Tension = 3;
			return;
		}
		Tension = amount;
	}

	public void SetWillAttack(bool willAttack)
		=> WillAttack = willAttack;
}
