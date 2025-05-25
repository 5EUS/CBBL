using CBBL.src.Pieces;

namespace CBBL.src.Interfaces;

public interface IBoard
{
    BoardState State { get; }

    IEnumerable<Move> GetLegalMoves();
    IEnumerable<Move> GetLegalMovesForPiece(int squareIndex);
    IEnumerable<Move> GetLegalMovesForPiece(PieceInfo piece);

    void Print();

    bool ExecuteMove(Move move);
    void UndoMove();
}