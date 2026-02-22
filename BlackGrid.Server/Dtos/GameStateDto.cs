using BlackGrid.Core.Cards;

namespace BlackGrid.Server.Dtos;

public record GameStateDto(
	Guid MatchId,
	int ActivePlayer,
	PlayerDto[] Players,
	int Phase
);

public record PlayerDto(
	int PlayerId,
	int Life,
	ColumnDto[] Columns,
	int HandSize,
	List<CardInstance> Hand
);

public record ColumnDto(
	CardDto? Front,
	CardDto? Back,
	bool WillAttack
);

public record CardDefinitionDto(
		string Id,
		string Name,
		string Description,
		int Attack
);

public record CardDto(
	string InstanceId,
	bool IsRevealed,
	CardDefinitionDto CardDefinition
);
