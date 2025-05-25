using CBBL.src.Pieces;

namespace CBBL.src.Interfaces;

public interface ILeapingAttackTables
{
    ulong[] KnightAttacks { get; }
    ulong[] KingAttacks { get; }
    ulong[] PawnAttacksWhite { get; }
    ulong[] PawnAttacksBlack { get; }

    List<ulong> Attacks { get; }
    ulong GetAttacksForPiece(PieceType pieceType, int square);
    
}