using CBBL.src.Board;
using CBBL.src.Debugging;
using CBBL.src.Interfaces;
using CBBL.src.Pieces;

namespace CBBL.src.Implementation;

public class CBBLBoard : IBoard
{
    public BoardState State { get; } = new CBBLBoardState();

    public bool ExecuteMove(Move move)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Move> GetLegalMoves()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Move> GetLegalMovesForPiece(int squareIndex)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Move> GetLegalMovesForPiece(PieceInfo piece)
    {
        throw new NotImplementedException();
    }

    public void UndoMove()
    {
        throw new NotImplementedException();
    }

    public void Print()
    {
        BoardUtils.PrintBoard(this);
    }
}