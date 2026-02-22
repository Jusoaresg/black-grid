using BlackGrid.Core.Board;
using BlackGrid.Core.Cards;
using BlackGrid.Core.Players;

namespace BlackGrid.Core.Combat;

public class CombatSystem
{
	public static void ResolveCombat(GameState state)
	{
		var attacker = state.ActualPlayer;
		var defender = state.OpponentPlayer;

		for (int i = 0; i < attacker.Board.Columns.Length; i++)
		{
			var attackColumn = attacker.Board.Columns[i];

			if (!attackColumn.WillAttack)
				continue;

			ResolveCombatAttack(attackColumn, defender.Board.Columns[i], attacker, defender);
		}

		foreach (var col in attacker.Board.Columns)
			col.SetWillAttack(false);
	}

	private static void ResolveCombatAttack(Column attackerColumn, Column defenderColumn, Player attacker, Player defender)
	{
		var attackerCard = attackerColumn.Front.Card;
		if (attackerCard == null)
			return;

		var attackValue = attackerCard.CardDefinition.Attack;

		var frontDefenseCard = defenderColumn.Front.Card;
		var backDefenseCard = defenderColumn.Back.Card;

		if (frontDefenseCard != null)
		{
			if (attackValue > frontDefenseCard.CardDefinition.Attack)
			{
				DestroyCard(frontDefenseCard, defenderColumn.Front, defender);

				var diff = attackValue - frontDefenseCard.CardDefinition.Attack;
				if (backDefenseCard != null)
				{
					if (diff > backDefenseCard.CardDefinition.Attack)
						DestroyCard(backDefenseCard, defenderColumn.Back, defender);
				}
				else
				{
					defender.TakeDamage(diff);
				}
			}
			else if (attackValue == frontDefenseCard.CardDefinition.Attack)
			{
				DestroyCard(frontDefenseCard, defenderColumn.Front, defender);
				DestroyCard(attackerCard, attackerColumn.Front, attacker);
			}
		}
		else if (backDefenseCard != null)
		{
			DestroyCard(backDefenseCard, defenderColumn.Back, defender);
		}
		else
		{
			defender.TakeDamage(attackValue);
		}
	}

	private static void DestroyCard(CardInstance card, Slot columnSlot, Player owner)
	{
		columnSlot.Clear();
		owner.Discard.Add(card);
		//NOTE: Remove this log later
		Console.WriteLine($"{card.CardDefinition.Name} was destroyed!");
	}
}
