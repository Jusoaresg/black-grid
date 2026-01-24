using BlackGrid.Core;
using BlackGrid.Core.Actions;
using BlackGrid.Core.Turn;
using BlackGrid.Game;

namespace BlackGrid.DebugConsole;

public class Program
{

	public static void Main()
	{
		var config = new GameConfig();
		var state = GameFactory.Create(config);
		var turnManager = new TurnManager(state);

		while (!state.GameOver)
		{
			turnManager.ResolvePhases();

			if (state.Phase == Phase.Action)
			{
				Console.WriteLine("Action phase...");
				var key = Console.ReadKey(true).KeyChar;


				if (key == '1')
				{
					foreach (var card in state.ActualPlayer.Hand)
					{
						Console.WriteLine(card.CardDefinition.Name);
					}
				}
				else if (key == '2')
				{
					var player = state.ActualPlayer;

					if (player.Hand.Count == 0)
					{
						Console.WriteLine("No cards in hand!");
						continue;
					}

					for (int i = 0; i < player.Hand.Count; i++)
						Console.WriteLine($"{i}: {player.Hand[i].CardDefinition.Name}");

					Console.Write("Choose card index: ");
					if (!int.TryParse(Console.ReadLine(), out int index) || index < 0 || index >= player.Hand.Count)
					{
						Console.WriteLine("Invalid index!");
						continue;
					}

					var card = player.Hand[index];
					var action = new PlayCardAction(state.ActualPlayerIndex, card.InstanceId, 0);
					turnManager.HandleAction(action);
				}
				else if (key == '3')
				{
					turnManager.HandleAction(new EndPhaseAction(state.ActualPlayerIndex));
				}

			}

			if (state.Phase == Phase.DeclareAttack)
			{
				Console.WriteLine("Select column to toggle attack (0-5):");
				var input = Console.ReadKey(true).KeyChar - '0';
				var action = new AttackCardAction(state.ActualPlayerIndex, input);
				turnManager.HandleAction(action);

				turnManager.HandleAction(new EndPhaseAction(state.ActualPlayerIndex));
			}
			PrintBoard(state);
		}

		Console.WriteLine(
		    state.WinnerPlayerIndex.HasValue
			? $"Winner: Player {state.WinnerPlayerIndex}"
			: "Draw"
		);
	}

	private static void PrintBoard(GameState state)
	{
		for (int p = 0; p < 2; p++)
		{
			var player = p == 0 ? state.Players[0] : state.Players[1];
			Console.WriteLine($"\nPlayer {player.Id} - Life: {player.Life} - Playing {player.Id == state.ActualPlayerIndex}");
			for (int i = 0; i < player.Board.Columns.Length; i++)
			{
				var col = player.Board.Columns[i];
				Console.WriteLine($" Column {i}: Front={col.Front.Card?.CardDefinition.Name ?? "empty"}, Back={col.Back.Card?.CardDefinition.Name ?? "empty"}");
			}
		}
	}
}
