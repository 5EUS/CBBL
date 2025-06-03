using System.Runtime.CompilerServices;

namespace CBBL.src.Pieces;

public enum PieceType
{
    WhitePawn = 0, WhiteKnight = 1, WhiteBishop = 2, WhiteRook = 3, WhiteQueen = 4, WhiteKing = 5,
    BlackPawn = 6, BlackKnight = 7, BlackBishop = 8, BlackRook = 9, BlackQueen = 10, BlackKing = 11,
    None = -1
}

public static class Pieces
{
    /// <summary>
    /// Starting bitboards for each piece type.
    /// </summary>

    public const ulong WHITE_PAWN_START = 0x000000000000FF00;
    public const ulong WHITE_ROOK_START = 0x0000000000000081;
    public const ulong WHITE_KNIGHT_START = 0x0000000000000042;
    public const ulong WHITE_BISHOP_START = 0x0000000000000024;
    public const ulong WHITE_QUEEN_START = 0x0000000000000008;
    public const ulong WHITE_KING_START = 0x0000000000000010;

    public const ulong BLACK_PAWN_START = 0x00FF000000000000;
    public const ulong BLACK_ROOK_START = 0x8100000000000000;
    public const ulong BLACK_KNIGHT_START = 0x4200000000000000;
    public const ulong BLACK_BISHOP_START = 0x2400000000000000;
    public const ulong BLACK_QUEEN_START = 0x0800000000000000;
    public const ulong BLACK_KING_START = 0x1000000000000000;

    public static PieceType[] WhitePieces => [PieceType.WhitePawn, PieceType.WhiteBishop, PieceType.WhiteKing, PieceType.WhiteKnight, PieceType.WhiteQueen, PieceType.WhiteRook];
    public static PieceType[] BlackPieces => [PieceType.BlackPawn, PieceType.BlackBishop, PieceType.BlackKing, PieceType.BlackKnight, PieceType.BlackQueen, PieceType.BlackRook];

    public static PieceType[] AllPieces => [PieceType.WhitePawn, PieceType.WhiteBishop, PieceType.WhiteKing, PieceType.WhiteKnight, PieceType.WhiteQueen, PieceType.WhiteRook,
                                            PieceType.BlackPawn, PieceType.BlackBishop, PieceType.BlackKing, PieceType.BlackKnight, PieceType.BlackQueen, PieceType.BlackRook];

    public static PieceType[] WhitePromotionPieces => [PieceType.WhiteQueen, PieceType.WhiteRook, PieceType.WhiteBishop, PieceType.WhiteKnight];
    public static PieceType[] BlackPromotionPieces => [PieceType.BlackQueen, PieceType.BlackRook, PieceType.BlackBishop, PieceType.BlackKnight];

    public static PieceType[] LeapingPieces => [PieceType.WhiteKnight, PieceType.BlackKnight,
                                                PieceType.WhiteKing, PieceType.BlackKing,
                                                PieceType.WhitePawn, PieceType.BlackPawn];

    public static PieceType[] SlidingPieces => [PieceType.WhiteBishop, PieceType.BlackBishop,
                                                  PieceType.WhiteRook, PieceType.BlackRook,
                                                  PieceType.WhiteQueen, PieceType.BlackQueen];                                            

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static PieceType CharToPieceType(char c)
    {
        return c switch
        {
            'P' => PieceType.WhitePawn,
            'N' => PieceType.WhiteKnight,
            'B' => PieceType.WhiteBishop,
            'R' => PieceType.WhiteRook,
            'Q' => PieceType.WhiteQueen,
            'K' => PieceType.WhiteKing,
            'p' => PieceType.BlackPawn,
            'n' => PieceType.BlackKnight,
            'b' => PieceType.BlackBishop,
            'r' => PieceType.BlackRook,
            'q' => PieceType.BlackQueen,
            'k' => PieceType.BlackKing,
            _ => throw new ArgumentException($"Invalid piece character: {c}", nameof(c))
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static char PieceTypeToChar(PieceType pieceIndex)
    {
        return pieceIndex switch
        {
            PieceType.WhitePawn => 'P',
            PieceType.WhiteKnight => 'N',
            PieceType.WhiteBishop => 'B',
            PieceType.WhiteRook => 'R',
            PieceType.WhiteQueen => 'Q',
            PieceType.WhiteKing => 'K',
            PieceType.BlackPawn => 'p',
            PieceType.BlackKnight => 'n',
            PieceType.BlackBishop => 'b',
            PieceType.BlackRook => 'r',
            PieceType.BlackQueen => 'q',
            PieceType.BlackKing => 'k',
            _ => throw new ArgumentOutOfRangeException(nameof(pieceIndex), "Invalid piece type")
        };
    }
}