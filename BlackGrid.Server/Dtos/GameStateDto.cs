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

public record CardDto(
	string Name,
	int Attack,
	int Defense,
	bool IsRevealed
);
