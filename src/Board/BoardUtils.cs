using System.Text;
using CBBL.src.Debugging;

namespace CBBL.src.Board;

public class BoardUtils
{
    public static string IndexToSquare(int index)
    {
        int file = index % BoardGlobals.Instance.NumFiles;
        int rank = index / BoardGlobals.Instance.NumRanks;
        char fileChar = (char)('a' + file);
        char rankChar = (char)('1' + rank);
        return $"{fileChar}{rankChar}";
    }

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