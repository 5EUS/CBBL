using CBBL.src.Pieces;

namespace CBBL.src.Interfaces;

public interface IBoard
{
    BoardState State { get; }
    void Print();

    bool ExecuteMove(Move move);
    void UndoMove();
}