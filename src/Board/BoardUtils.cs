using System.Runtime.CompilerServices;
using System.Text;
using CBBL.src.Debugging;
using CBBL.src.Interfaces;
using CBBL.src.Pieces;

namespace CBBL.src.Board;

/// <summary>
/// Conversion and interfacing tools for bitboards
/// </summary>
public class BoardUtils
{
    /// <summary>
    /// Integer index to square ID (eg. e4)
    /// </summary>
    /// <param name="index">The integer index of the square</param>
    /// <returns>The square ID</returns>
    public static string IndexToSquare(int index)
    {
        int file = index % BoardGlobals.Instance.NumFiles;
        int rank = index / BoardGlobals.Instance.NumRanks;
        char fileChar = (char)('a' + file);
        char rankChar = (char)('1' + rank);
        return $"{fileChar}{rankChar}";
    }

    /// <summary>
    /// Square ID (eg e4) to integer index
    /// </summary>
    /// <param name="square">The ID of the square</param>
    /// <returns>Integer index of the given square</returns>
    public static int SquareToIndex(string square)
    {
        if (square.Length != 2)
        {
            Logger.DualLogLine($"Invalid square format: {square}", LogLevel.Warning);
            return -1;
        }

        char fileChar = square[0];
        char rankChar = square[1];

        int file = fileChar - 'a'; // a=0, h=7
        int rank = rankChar - '1'; // 1=0, 8=7

        if (file < 0 || file > BoardGlobals.Instance.NumFiles - 1
            || rank < 0
            || rank > BoardGlobals.Instance.NumRanks - 1)
        {
            Logger.DualLogLine($"Invalid square: {square}", LogLevel.Warning);
            return -1;
        }
        return rank * BoardGlobals.Instance.NumRanks + file;
    }

    /// <summary>
    /// Prints a stylized given bitboard in the form of a ulong
    /// </summary>
    /// <param name="bitboard">The bitboard to print</param>
    /// <param name="level">Optional: log level to print as</param>
    public static void PrintBitboard(ulong bitboard, LogLevel level = LogLevel.None)
    {
        Console.Write(GetBitboardString(bitboard));
    }

    /// <summary>
    /// Convert a bitboard into an unformatted string representation
    /// </summary>
    /// <param name="bitboard">The bitboard to convert</param>
    /// <returns></returns>
    public static string GetBitboardString(ulong bitboard)
    {
        StringBuilder sb = new();
        for (int rank = 7; rank >= 0; rank--)
        {
            sb.Append($"{rank + 1} ");
            for (int file = 0; file < 8; file++)
            {
                int square = rank * 8 + file;
                ulong mask = 1UL << square;
                sb.Append((bitboard & mask) != 0 ? "1 " : ". ");
            }
            sb.AppendLine();
        }
        sb.AppendLine("  a b c d e f g h");
        return sb.ToString();
    }

    public static void PrintBoard(IBoard board)
    {
        Console.Write(GetBoardString(board));
    }

    public static string GetBoardString(IBoard board)
    {
        StringBuilder sb = new();
        sb.AppendLine();
        for (int rank = 7; rank >= 0; rank--)
        {
            sb.Append($"{rank + 1} ");
            for (int file = 0; file < 8; file++)
            {
                int square = rank * 8 + file;
                sb.Append(GetPieceSymbol(board, square) + " ");
            }
            sb.AppendLine();
        }
        sb.AppendLine("  a b c d e f g h");
        sb.AppendLine();
        return sb.ToString();
    }

    public static char GetPieceSymbol(IBoard board, int square)
    {
        for (int i = 0; i < BoardGlobals.Instance.NumPieces; i++)
        {
            if (((board.State.Bitboards[i] >> square) & 1) != 0)
            {
                return PieceToChar((PieceType)i);
            }
        }
        return '.';
    }

    private static char PieceToChar(PieceType piece)
    {
        return piece switch
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
            _ => '.'
        };
    }

    public static PlayerColor BoolToPlayerColor(bool playerColor)
    {
        return playerColor ? PlayerColor.White : PlayerColor.Black;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong AllPieces(BoardState boardState)
    {
        var boards = boardState.Bitboards;
        return boards[0] | boards[1] | boards[2] | boards[3] | boards[4] | boards[5] | boards[6]
            | boards[7] | boards[8] | boards[9] | boards[10] | boards[11];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong WhitePieces(BoardState boardState)
    {
        var boards = boardState.Bitboards;
        return boards[0] | boards[1] | boards[2] | boards[3] | boards[4] | boards[5];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong BlackPieces(BoardState boardState)
    {
        var boards = boardState.Bitboards;
        return boards[6] | boards[7] | boards[8] | boards[9] | boards[10] | boards[11];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong Empties(BoardState boardState)
    {
        return ~(WhitePieces(boardState) | BlackPieces(boardState));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong Pawns(BoardState boardState)
    {
        return boardState.Bitboards[(int)PieceType.WhitePawn] 
             | boardState.Bitboards[(int)PieceType.BlackPawn];
    }
}