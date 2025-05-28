using CBBL.src.Board;
using CBBL.src.Interfaces;
using CBBL.src.Pieces;

namespace CBBL.src.Implementation;

public class CBBLBoardState : BoardState
{
    internal override ulong[] Bitboards { get; } = new ulong[BoardGlobals.Instance.NumPieces];

    public override PlayerColor ActivePlayer { get; } = PlayerColor.White;

    public override int HalfMoveClock => throw new NotImplementedException();

    public override int FullMoveNumber => throw new NotImplementedException();

    public override int? EnPassantSquare { get; } = null;

    public override PlayerColor ColorToMove { get; } = PlayerColor.White;

    public CBBLBoardState()
    {
        Init();
    }

    public override bool CanCastleKingside(bool isWhite)
    {
        throw new NotImplementedException();
    }

    public override bool CanCastleQueenside(bool isWhite)
    {
        throw new NotImplementedException();
    }

    public override ulong GetBitboardFor(PieceType type)
    {
        return Bitboards[(int)type];
    }

    public override string GetFen()
    {
        throw new NotImplementedException();
    }

    public override PieceInfo? GetPieceAt(int squareIndex)
    {
        throw new NotImplementedException();
    }

    public override bool IsKingInCheck(PlayerColor playerColor)
    {
        throw new NotImplementedException();
    }
}