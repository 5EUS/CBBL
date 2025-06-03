using CBBL.src.Board;
using CBBL.src.Pieces;

namespace CBBL.src.AI;

public static class PositionTable
{

    private static readonly int[,] PawnTable = {
        {  0,  0,  0,  0,  0,  0,  0,  0 },
        {  2,  5,  5,-10,-10,  5,  5,  2 },
        {  2, -2, -5,  0,  0, -5, -2,  2 },
        {  0,  0,  0, 10, 10,  0,  0,  0 },
        {  2,  2,  5, 12, 12,  5,  2,  2 },
        {  5,  5, 10, 15, 15, 10,  5,  5 },
        { 10, 10, 10, 10, 10, 10, 10, 10 },
        {  0,  0,  0,  0,  0,  0,  0,  0 }
    };

    private static readonly int[,] KnightTable = {
        { -50,-40,-30,-30,-30,-30,-40,-50 },
        { -40,-20,  0,  0,  0,  0,-20,-40 },
        { -30,  0, 10, 15, 15, 10,  0,-30 },
        { -30,  5, 15, 20, 20, 15,  5,-30 },
        { -30,  0, 15, 20, 20, 15,  0,-30 },
        { -30,  5, 10, 15, 15, 10,  5,-30 },
        { -40,-20,  0,  5,  5,  0,-20,-40 },
        { -50,-40,-30,-30,-30,-30,-40,-50 }
    };

    private static readonly int[,] KingTable = {
        { -30,-40,-40,-50,-50,-40,-40,-30 },
        { -30,-40,-40,-50,-50,-40,-40,-30 },
        { -30,-40,-40,-50,-50,-40,-40,-30 },
        { -30,-40,-40,-50,-50,-40,-40,-30 },
        { -20,-30,-30,-40,-40,-30,-30,-20 },
        { -10,-20,-20,-20,-20,-20,-20,-10 },
        {  20, 20,  0,  0,  0,  0, 20, 20 },
        {  20, 30, 10,  0,  0, 10, 30, 20 }
    };

    private static readonly int[,] BishopTable = {
        { -20,-10,-10,-10,-10,-10,-10,-20 },
        { -10,  5,  0,  0,  0,  0,  5,-10 },
        { -10, 10, 10, 10, 10, 10, 10,-10 },
        { -10,  0, 10, 10, 10, 10,  0,-10 },
        { -10,  5,  5, 10, 10,  5,  5,-10 },
        { -10,  0,  5, 10, 10,  5,  0,-10 },
        { -10,  0,  0,  0,  0,  0,  0,-10 },
        { -20,-10,-10,-10,-10,-10,-10,-20 }
    };

    private static readonly int[,] RookTable = {
        {  0,  0,  0,  15, 15,  0,  0,  0 },
        { -5,  0,  0,  0,  0,  0,  0, -5 },
        { -5,  0,  0,  0,  0,  0,  0, -5 },
        { -5,  0,  0,  0,  0,  0,  0, -5 },
        { -5,  0,  0,  0,  0,  0,  0, -5 },
        { -5,  0,  0,  0,  0,  0,  0, -5 },
        {  5, 10, 10, 15, 15, 10, 10,  5 },
        {  0,  0,  0,  5,  5,  0,  0,  0 }
    };

    private static readonly int[,] QueenTable = {
        { -30,-20,-20, -10, -10,-20,-20,-30 },
        { -20,  0,  10,  0,  0,  0,  0,-20 },
        { -20,  10,  10,  10,  10,  10,  0,-20 },
        {  -10,  0,  10,  10,  10,  10,  0, -10 },
        {   0,  0,  10,  10,  10,  10,  0, -10 },
        { -20,  10,  10,  10,  10,  10,  0,-20 },
        { -20,  0,  10,  0,  0,  0,  0,-20 },
        { -30,-20,-20, -10, -10,-20,-20,-30 }
    };

    public static int GetValue(PieceType pieceType, int position)
    {
        int rank = position / BoardGlobals.Instance.NumFiles;
        int file = position % BoardGlobals.Instance.NumFiles;

        return pieceType switch
        {
            PieceType.WhitePawn => PawnTable[rank, file],
            PieceType.BlackPawn => PawnTable[7 - rank, file],
            PieceType.WhiteKnight => KnightTable[rank, file],
            PieceType.BlackKnight => KnightTable[7 - rank, file],
            PieceType.WhiteBishop => BishopTable[rank, file],
            PieceType.BlackBishop => BishopTable[7 - rank, file],
            PieceType.WhiteRook => RookTable[rank, file],
            PieceType.BlackRook => RookTable[7 - rank, file],
            PieceType.WhiteQueen => QueenTable[rank, file],
            PieceType.BlackQueen => QueenTable[7 - rank, file],
            PieceType.WhiteKing => KingTable[rank, file],
            PieceType.BlackKing => KingTable[7 - rank, file],
            _ => 0
        };
    }
}