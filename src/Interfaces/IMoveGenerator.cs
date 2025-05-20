using CBBL.src.Pieces;

namespace CBBL.src.Interfaces;

public interface IMoveGenerator
{
    List<Move> GenerateMoves(IBoard board);
}