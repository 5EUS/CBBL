using CBBL.src.Pieces;

namespace CBBL.src.Interfaces;

public interface IMagicGenerator
{
    SlidingPieceHandler.Result Magics { get; }
}