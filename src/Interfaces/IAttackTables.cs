using CBBL.src.Pieces;

namespace CBBL.src.Interfaces;

public interface IAttackTables
{
    ILeapingAttackTables LeapingAttackTables { get; }
    IMagicGenerator MagicGenerator { get; }
    ulong Aggregate(BoardState boardState, PieceType[] types);
}
