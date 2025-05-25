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
        Logger.LogLine();
        for (int rank = 7; rank >= 0; rank--)
        {
            Logger.Log($"{rank + 1} ");
            for (int file = 0; file < 8; file++)
            {
                int square = rank * 8 + file;
                Logger.Log(BoardUtils.GetPieceSymbol(this, square) + " ");
            }
            Logger.LogLine();
        }
        Logger.LogLine("  a b c d e f g h");
        Logger.LogLine();
    }
}