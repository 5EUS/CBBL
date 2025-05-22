using CBBL.src.Board;
using CBBL.src.Interfaces;
using CBBL.src.Pieces;

namespace CBBL.src.Implementation;

public class CBBLBoardState : BoardState
{
    public override ulong[] Bitboards { get; } = new ulong[BoardGlobals.Instance.NumPieces];

    public override PlayerColor ActivePlayer => throw new NotImplementedException();

    public override bool IsWhiteToMove => throw new NotImplementedException();

    public override int HalfMoveClock => throw new NotImplementedException();

    public override int FullMoveNumber => throw new NotImplementedException();

    public override int? EnPassantSquare => throw new NotImplementedException();

    public override bool CanCastleKingside(bool isWhite)
    {
        throw new NotImplementedException();
    }

    public override bool CanCastleQueenside(bool isWhite)
    {
        throw new NotImplementedException();
    }

    public override ulong GetBitboardFor(PieceType type, bool isWhite)
    {
        throw new NotImplementedException();
    }

    public override string GetFen()
    {
        throw new NotImplementedException();
    }

    public override PieceInfo? GetPieceAt(int squareIndex)
    {
        throw new NotImplementedException();
    }
}