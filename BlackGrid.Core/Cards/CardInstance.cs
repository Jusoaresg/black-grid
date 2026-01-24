using BlackGrid.Core.Players;

namespace BlackGrid.Core.Cards;

public class CardInstance(string instanceId, CardDefinition card)
{
	public string InstanceId { get; } = instanceId;
	public CardDefinition CardDefinition { get; } = card;
	public bool IsExhausted { get; set; }
	public bool IsRevealed { get; set; }
}
