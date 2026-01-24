namespace BlackGrid.Core;

public class GameConfig
{
	public int PlayerCount = 2;
	public int BoardColumns = 6;

	public int[] DeckIds { get; set; } = new int[2];

	public bool Debug;
}
