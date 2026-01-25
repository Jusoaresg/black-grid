namespace BlackGrid.Server.Infrastructure;

public static class ServerPaths
{
	public static string DataRootPath { get; private set; } = null!;

	public static void Init(string dataRootPath)
	{
		DataRootPath = Path.Combine(dataRootPath, "..", "BlackGrid.Data");
	}
}
