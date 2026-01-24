using System.Text.Json;
using BlackGrid.Core.Decks;

namespace BlackGrid.Data.Databases;

public class DeckDatabase
{
	private readonly Dictionary<string, DeckDefinition> _decks;

	public DeckDatabase(string decksRootFolder, CardDatabase cardDb)
	{
		_decks = [];
		var options = JsonOptions.GetJsonOptions();

		foreach (var file in Directory.GetFiles(decksRootFolder, "*.json", SearchOption.AllDirectories))
		{
			var json = File.ReadAllText(file);
			var deck = JsonSerializer.Deserialize<DeckDefinition>(json, options);

			if (deck == null)
				throw new Exception($"Invalid deck file: {deck}");

			foreach (var card in deck.CardIds)
			{
				if (!cardDb.Exists(card))
					throw new Exception($"Deck {deck.Id} references unknown card {card}");
			}

			_decks.Add(deck.Id, deck);
		}
	}

	public DeckDefinition Get(string id)
		=> _decks[id];

	public bool Exists(string id)
		=> _decks.ContainsKey(id);
}
