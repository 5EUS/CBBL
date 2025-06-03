using System.Text;
using CBBL.src.Board;

namespace CBBL.src.Pieces;

/// <summary>
/// Flags for determining the type of move.
/// </summary>
public enum MoveFlag
{
    None = 0,
    Capture = 1,
    Promotion = 2,
    EnPassant = 4,
    Castling = 8,
    Check = 16,
    Checkmate = 32,
    DoublePush = 64
}

/// <summary>
/// Data for a move
/// </summary>
/// <param name="from"></param>
/// <param name="to"></param>
public struct Move(int from, int to, MoveFlag flag = MoveFlag.None, PieceType? promotionPieceType = null)
{
    public int FromSquare { get; } = from;
    public int ToSquare { get; } = to;
    public MoveFlag Flag { get; set; } = flag;
    public PieceType? PromotionPieceType { get; set; } = promotionPieceType;
}

public class Moves
{
    public static string MoveToString(Move move)
    {
        StringBuilder sb = new();
        sb.AppendLine("From:");
        sb.Append(BoardUtils.GetBitboardString(BoardOps.SetBit(0UL, move.FromSquare)));
        sb.AppendLine("To:");
        sb.Append(BoardUtils.GetBitboardString(BoardOps.SetBit(0UL, move.ToSquare)));
        return sb.ToString();
    }

    public static bool MovesContainIgnoreFlags(IEnumerable<Move> moves, Move move)
    {
        foreach (var m in moves)
        {
            if (m.FromSquare == move.FromSquare && m.ToSquare == move.ToSquare)
            {
                return true;
            }
        }
        return false;
    }

    public static bool EqualsIgnoreFlags(Move m, Move inMove)
    {
        return m.FromSquare == inMove.FromSquare && m.ToSquare == inMove.ToSquare;
    }
}