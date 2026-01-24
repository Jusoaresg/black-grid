using BlackGrid.Core.Board;
using BlackGrid.Core.Cards;
using BlackGrid.Core.Decks;

namespace BlackGrid.Core.Players;

public class Player(
	int id,
	Deck deck,
	PlayerBoard board,
	int startingLife = 20000
    )
{
	public int Id { get; set; } = id;
	public int Life { get; private set; } = startingLife;

	public Deck Deck { get; } = deck;
	public IList<CardInstance> Hand { get; } = new List<CardInstance>();
	public IList<CardInstance> Discard { get; } = new List<CardInstance>();
	public IList<CardInstance> Exile { get; } = new List<CardInstance>();

	public PlayerBoard Board { get; } = board;

	public void TakeDamage(int damage)
		=> Life -= damage;
}
