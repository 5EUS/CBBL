using CBBL.src.Board;
using CBBL.src.Pieces;
using static CBBL.src.Pieces.Pieces;

namespace CBBL.src.Interfaces;

public abstract class BoardState
{
    internal abstract ulong[] Bitboards { get; }

    public abstract PlayerColor ActivePlayer { get; }

    public abstract PlayerColor ColorToMove { get; }
    public abstract int HalfMoveClock { get; }
    public abstract int FullMoveNumber { get; }

    // Piece positions (mapped per square or bitboard)
    public abstract PieceInfo? GetPieceAt(int squareIndex);

    public abstract bool CanCastleKingside(bool isWhite);
    public abstract bool CanCastleQueenside(bool isWhite);
    public abstract bool IsKingInCheck(PlayerColor playerColor);
    public abstract ulong? EnPassantSquare { get; }

    public abstract ulong GetBitboardFor(PieceType type);

    public abstract string GetFen();

    protected virtual void Init()
    {
        var bitboards = Bitboards;

        bitboards[(int)PieceType.WhitePawn] = WHITE_PAWN_START;
        bitboards[(int)PieceType.WhiteKnight] = WHITE_KNIGHT_START;
        bitboards[(int)PieceType.WhiteBishop] = WHITE_BISHOP_START;
        bitboards[(int)PieceType.WhiteRook] = WHITE_ROOK_START;
        bitboards[(int)PieceType.WhiteQueen] = WHITE_QUEEN_START;
        bitboards[(int)PieceType.WhiteKing] = WHITE_KING_START;

        bitboards[(int)PieceType.BlackPawn] = BLACK_PAWN_START;
        bitboards[(int)PieceType.BlackKnight] = BLACK_KNIGHT_START;
        bitboards[(int)PieceType.BlackBishop] = BLACK_BISHOP_START;
        bitboards[(int)PieceType.BlackRook] = BLACK_ROOK_START;
        bitboards[(int)PieceType.BlackQueen] = BLACK_QUEEN_START;
        bitboards[(int)PieceType.BlackKing] = BLACK_KING_START; 
    }
}
