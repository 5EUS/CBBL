using CBBL.src.Pieces;

namespace CBBL.src.Interfaces;

public interface IPieceMoveGenerator
{
    IEnumerable<Move> GenerateMoves(PieceType pieceType);
}