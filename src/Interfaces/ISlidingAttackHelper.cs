namespace CBBL.src.Interfaces;

public interface ISlidingAttackHelper
{
    ulong GetBishopAttacks(int squareIndex, ulong blockers);
    ulong GetQueenAttacks(int squareIndex, ulong blockers);
    ulong GetRookAttacks(int squareIndex, ulong blockers);
}