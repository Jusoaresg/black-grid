using System.Text.Json;
using BlackGrid.Core.Cards;

namespace BlackGrid.Data.Databases;

public class CardDatabase
{
	private readonly Dictionary<string, CardDefinition> _cards;

	public CardDatabase(string cardsRootPath)
	{
		_cards = [];
		var options = JsonOptions.GetJsonOptions();

		foreach (var file in Directory.GetFiles(cardsRootPath, "*.json", SearchOption.AllDirectories))
		{
			var json = File.ReadAllText(file);
			var card = JsonSerializer.Deserialize<CardDefinition>(json, options);

			if (card == null)
				throw new Exception($"Invalid card file: {card}");

			if (_cards.ContainsKey(card.Id))
				throw new Exception($"Duplicate card id: {card.Id}");

			_cards.Add(card.Id, card);
		}
	}

	public CardDefinition Get(string id)
		=> _cards[id];

	public bool Exists(string id)
		=> _cards.ContainsKey(id);
}
