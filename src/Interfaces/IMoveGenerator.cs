using CBBL.src.Board;
using CBBL.src.Pieces;

namespace CBBL.src.Interfaces;


public enum MoveType
{
    LEGAL,
    PSEUDO_LEGAL,
    EVASION
}

public interface IMoveGenerator
{
    IPieceMoveGenerator Generator { get; }

    IEnumerable<Move> GenerateMoves(BoardState boardState, MoveType moveType, PlayerColor? color = null);
    bool TryGetMoveIgnoreFlags(IEnumerable<Move> moves, Move inMove, out Move move)
    {
        if (Moves.MovesContainIgnoreFlags(moves, inMove))
        {
            move = moves.First(m => Moves.EqualsIgnoreFlags(m, inMove));
            return true;
        }
        move = default;
        return false;
    }

    bool TryGetMove(IEnumerable<Move> moves, Move inMove, out Move move)
    {
        if (moves.Contains(inMove))
        {
            move = inMove;
            return true;
        }
        move = default;
        return false;
    }

    bool TryGetPromotionMove(IEnumerable<Move> moves, Move inMove, PieceType pieceType, out Move move)
    {
        if (moves.Any(m => Moves.EqualsIgnoreFlags(m, inMove) && m.PromotionPieceType == pieceType))
        {
            move = moves.First(m => Moves.EqualsIgnoreFlags(m, inMove) && m.PromotionPieceType == pieceType);
            return true;
        }
        move = default;
        return false;
    }
}