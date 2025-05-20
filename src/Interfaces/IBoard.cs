using CBBL.src.Pieces;

namespace CBBL.src.Interfaces;

public interface IBoard
{
    IBoardState BoardState { get; }

    IEnumerable<Move> GetLegalMoves();
    IEnumerable<Move> GetLegalMovesForPiece(int squareIndex);
    IEnumerable<Move> GetLegalMovesForPiece(PieceInfo piece);

    void ExecuteMove(Move move);
    void UndoMove();
}