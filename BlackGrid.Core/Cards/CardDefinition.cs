using System.Text.Json.Serialization;

namespace BlackGrid.Core.Cards;

public class CardDefinition
{
	[JsonPropertyName("id")]
	public required string Id { get; set; }

	[JsonPropertyName("name")]
	public required string Name { get; set; }

	[JsonPropertyName("description")]
	public string? Description { get; set; }

	[JsonPropertyName("attack")]
	public required int Attack { get; set; }

	[JsonPropertyName("defense")]
	public required int Defense { get; set; }

	[JsonPropertyName("type")]
	public required CardType Type { get; set; }

	// public IList<CardEffect> Effects { get; set; }
}
