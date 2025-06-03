using CBBL.src.Pieces;

namespace CBBL.src.Interfaces;

public interface IPieceMoveGenerator
{
    void GenerateMoves(ref List<Move> result, BoardState boardState, PieceType pieceType);
}