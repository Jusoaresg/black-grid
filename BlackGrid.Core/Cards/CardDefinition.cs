namespace BlackGrid.Core.Cards;

public class CardDefinition
{
	public required string Id { get; set; }

	public required string Name { get; set; }

	public string? Description { get; set; }

	public required int Attack { get; set; }

	public required int Defense { get; set; }

	public required CardType Type { get; set; }

	// public IList<CardEffect> Effects { get; set; }
}
