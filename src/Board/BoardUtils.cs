using System.Text;
using CBBL.src.Debugging;

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
        for (int rank = 7; rank >= 0; rank--)
        {
            Logger.Log($"{rank + 1} ", level);
            for (int file = 0; file < 8; file++)
            {
                int square = rank * 8 + file;
                ulong mask = 1UL << square;
                Logger.Log((bitboard & mask) != 0 ? "1 " : ". ");
            }
            Logger.LogLine(level: level);
        }
        Logger.LogLine("  a b c d e f g h", level);
        Logger.LogLine(level: level);
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
}