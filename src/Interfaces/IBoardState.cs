using CBBL.src.Board;
using CBBL.src.Pieces;

namespace CBBL.src.Interfaces;

public interface IBoardState
{

    ulong[] Bitboards { get; }

    PlayerColor ActivePlayer { get; }

    bool IsWhiteToMove { get; }
    int HalfMoveClock { get; }
    int FullMoveNumber { get; }

    IMoveGenerator MoveGenerator { get; }

    // Piece positions (mapped per square or bitboard)
    PieceInfo? GetPieceAt(int squareIndex);

    bool CanCastleKingside(bool isWhite);
    bool CanCastleQueenside(bool isWhite);
    int? EnPassantSquare { get; }

    ulong GetBitboardFor(PieceType type, bool isWhite);

    string GetFen();
}
