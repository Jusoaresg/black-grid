namespace BlackGrid.Core.Actions;

public interface IGameAction
{
	int PlayerId { get; }
	bool CanExecute(GameState state);
	void Execute(GameState state);
}
