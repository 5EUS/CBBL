using CBBL.src.Board;
using CBBL.src.Pieces;

namespace CBBL.src.Interfaces;

public interface ILeapingAttackTables
{
    ulong GetAttacksForPiece(PieceType pieceType, int square);
    
}

public interface ILeapingAttackData
{
    ulong[] KnightAttacks { get; }
    ulong[] KingAttacks { get; }
    ulong[] PawnAttacksWhite { get; }
    ulong[] PawnAttacksBlack { get; }
}

public readonly struct LeapingAttackData : ILeapingAttackData
{
    public LeapingAttackData()
    {
    }

    public ulong[] KnightAttacks { get;} = new ulong[BoardGlobals.Instance.NumSquares];

    public ulong[] KingAttacks { get;} = new ulong[BoardGlobals.Instance.NumSquares];

    public ulong[] PawnAttacksWhite { get; } = new ulong[BoardGlobals.Instance.NumSquares];

    public ulong[] PawnAttacksBlack { get;} = new ulong[BoardGlobals.Instance.NumSquares];
}