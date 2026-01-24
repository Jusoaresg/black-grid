namespace BlackGrid.Core.Board;

public class PlayerBoard
{
	public Column[] Columns { get; }

	public PlayerBoard(int columnNumber = 6)
	{
		Columns = new Column[columnNumber];
		for (int i = 0; i < columnNumber; i++)
		{
			Columns[i] = new Column();
		}
	}
}
