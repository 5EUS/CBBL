using CBBL.src.Implementation;
using CBBL.src.Pieces;

namespace CBBL.src.Interfaces;

public interface IMagicGenerator
{
    SlidingPieceHandler.Result Magics { get; }
    RookMagics RookMagics { get; }
    BishopMagics BishopMagics { get; }

    ulong GetRookAttacks(int square, ulong blockers);
    ulong GetBishopAttacks(int square, ulong blockers);
}