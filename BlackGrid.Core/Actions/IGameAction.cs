namespace BlackGrid.Core.Actions;

public interface IGameAction
{
	int PlayerId { get; }
	void Execute(GameState state);
}
