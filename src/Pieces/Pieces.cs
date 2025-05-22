namespace CBBL.src.Pieces;

public enum PieceType
{
    WhitePawn, WhiteKnight, WhiteBishop, WhiteRook, WhiteQueen, WhiteKing,
    BlackPawn, BlackKnight, BlackBishop, BlackRook, BlackQueen, BlackKing
}

public readonly struct PieceInfo
{
    public PieceType Type { get; init; }
    public bool IsWhite { get; init; }
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

}