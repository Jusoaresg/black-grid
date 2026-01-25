using System.Net.WebSockets;
using System.Text;
using BlackGrid.Server.Matchmaking;
using BlackGrid.Server.Session;

namespace BlackGrid.Server.Websockets;

public static class WebsocketReceiver
{
	public static async Task ReceiveLoop(PlayerSession session)
	{
		var buffer = new byte[4096];

		try
		{
			while (session.Socket.State == WebSocketState.Open)
			{
				var result = await session.Socket.ReceiveAsync(
						buffer,
						CancellationToken.None
						);

				if (result.MessageType == WebSocketMessageType.Close)
					break;

				var json = Encoding.UTF8.GetString(buffer, 0, result.Count);
				await MessageHandler.HandleMesssage(session, json);
			}
		}
		catch (OperationCanceledException)
		{
			Console.WriteLine($"[WS] Receive canceled | Session={session.SessionId}");
		}
		catch (Exception e)
		{
			Console.WriteLine($"[WS] Fatal error | Session={session.SessionId}");
			Console.WriteLine(e);
		}
		finally
		{
			await CleanupSession(session);
		}
	}

	private static async Task CleanupSession(PlayerSession session)
	{
		Console.WriteLine($"[WS] Cleanup | Session={session.SessionId}");
		MatchmakingService.Instance.Remove(session);

		if (session.CurrentMatch != null)
		{
			Console.WriteLine($"[MATCH] Player disconnected | Match={session.CurrentMatch.MatchId}");
			await session.CurrentMatch.HandleDisconnect(session);
		}

		try
		{
			if (session.Socket.State is WebSocketState.Open or WebSocketState.CloseReceived)
			{
				await session.Socket.CloseAsync(
					WebSocketCloseStatus.NormalClosure,
					"Connection closed",
					CancellationToken.None
				);
			}
		}
		catch { }
	}

}
