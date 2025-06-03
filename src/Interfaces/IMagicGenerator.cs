using CBBL.src.Pieces;

namespace CBBL.src.Interfaces;

public interface IMagicGenerator
{
    ulong GetRookMagic(int square);
    ulong GetBishopMagic(int square);

    ulong GetRookAttacks(int square, ulong blockers);
    ulong GetBishopAttacks(int square, ulong blockers);
    ulong GetQueenAttacks(int square, ulong blockers);
    
    ulong GetAttacksForPiece(PieceType pieceType, int square, ulong blockers);
}