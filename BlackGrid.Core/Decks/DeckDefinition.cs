using System.Text.Json.Serialization;

namespace BlackGrid.Core.Decks;

public class DeckDefinition
{
	public required string Id { get; set; }
	public required string Name { get; set; }
	public string? Description { get; set; }

	[JsonPropertyName("cards")]
	public required IReadOnlyList<string> CardIds { get; init; }
}
