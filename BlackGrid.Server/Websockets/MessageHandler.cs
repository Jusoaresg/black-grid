using System.Text;
using System.Text.Json;
using BlackGrid.Core.Actions;
using BlackGrid.Core.Turn;
using BlackGrid.Data;
using BlackGrid.Server.Matchmaking;
using BlackGrid.Server.Session;

namespace BlackGrid.Server.Websockets;

public record BaseMessage(string Type);
public record IngoingMessage<T>(string Type, T Payload) : BaseMessage(Type);
public record ActionDto(Guid CardInstanceId, int ColumnId);
public record AttackDto(int ColumnId);

public static class MessageHandler
{
	public static async Task HandleMesssage(PlayerSession session, string json)
	{
		var msg = JsonSerializer.Deserialize<BaseMessage>(json, JsonOptions.Options);

		switch (msg?.Type)
		{
			case "find_match":
				if (session.CurrentMatch == null)
					await HandleFindMatch(session);
				break;

			case "action":
				var actionDto = JsonSerializer.Deserialize<IngoingMessage<ActionDto>>(json, JsonOptions.Options);

				// INFO: Logger here
				if (actionDto?.Payload == null)
					return;

				await HandleAction(session, actionDto.Payload);
				break;

			case "attack":
				var attackDto = JsonSerializer.Deserialize<IngoingMessage<AttackDto>>(json, JsonOptions.Options);

				// INFO: Logger here
				if (attackDto?.Payload == null)
					return;

				await HandleAttack(session, attackDto.Payload);
				break;
		}
	}

	private static async Task HandleFindMatch(PlayerSession session)
	{
		var match = MatchmakingService.Instance.TryCreateMatch(session);

		if (match is null)
		{
			var message = new
			{
				type = "matchmaking",
				payload = new
				{
					status = "waiting"
				}
			};
			var json = JsonSerializer.Serialize(message, JsonOptions.Options);
			var bytes = Encoding.UTF8.GetBytes(json);
			await MatchNotifier.Send(session, bytes);
		}
		else
		{
			match.TurnManager.ResolvePhases();
			await MatchNotifier.BroadcastMatchStart(match);
		}
	}

	private static async Task HandleAction(PlayerSession session, ActionDto actionDto)
	{
		if (session.CurrentMatch == null)
			return;

		var player = session.CurrentMatch.GetPlayerIndex(session.UserId);
		await session.CurrentMatch.HandleAction(session.UserId, new PlayCardAction(player, actionDto.CardInstanceId.ToString(), actionDto.ColumnId));
	}

	private static async Task HandleAttack(PlayerSession session, AttackDto attackDto)
	{
		if (session.CurrentMatch == null)
			return;

		var player = session.CurrentMatch.GetPlayerIndex(session.UserId);
		await session.CurrentMatch.HandleAction(session.UserId, new AttackCardAction(player, attackDto.ColumnId));
	}
}
