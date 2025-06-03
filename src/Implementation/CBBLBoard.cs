using CBBL.src.Board;
using CBBL.src.Interfaces;
using CBBL.src.Pieces;

namespace CBBL.src.Implementation;

public class CBBLBoard : IBoard
{
    public CBBLBoard(string? fen = null)
    {
        State = new CBBLBoardState(fen);
    }

    public CBBLBoard(PlayerColor color)
    {
        State = new CBBLBoardState(color);
    }

    public BoardState State { get; }

    public bool ExecuteMove(Move move)
    {
        return State.ExecuteMove(move);
    }

    public void UndoMove()
    {
        State.UndoMove();
    }

    public void Print()
    {
        BoardUtils.PrintBoard(this);
    }
}