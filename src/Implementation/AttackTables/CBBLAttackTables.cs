using CBBL.src.Board;
using CBBL.src.Implementation.AttackTables.Magic;
using CBBL.src.Interfaces;
using CBBL.src.Pieces;

namespace CBBL.src.Implementation.AttackTables;

public class CBBLAttackTables : IAttackTables
{
    public ILeapingAttackTables LeapingAttackTables { get; }
    public IMagicGenerator MagicGenerator { get; }

    public ulong[] KingDangerMask { get; }

    public CBBLAttackTables(int debug)
    {
        LeapingAttackTables = new CBBLLeapingAttackTables();
        var lat = LeapingAttackTables as CBBLLeapingAttackTables;
        MagicGenerator = new CBBLMagicGenerator(debug);
        KingDangerMask = new ulong[BoardGlobals.Instance.NumSquares];
        for (int square = 0; square < BoardGlobals.Instance.NumSquares; square++)
        {
            KingDangerMask[square] = lat.Data.KnightAttacks[square] | lat.Data.PawnAttacksWhite[square]
                | lat.Data.PawnAttacksBlack[square] | MagicGenerator.GetRookAttacks(square, 0UL) | MagicGenerator.GetBishopAttacks(square, 0UL);
        }
    }


    public ulong Aggregate(BoardState boardState, PieceType[] types)
    {
        ulong result = 0UL;
        var allPieces = BoardUtils.AllPieces(boardState);
        foreach (var type in types)
        {
            var occupancy = boardState.GetBitboardFor(type);
            while (occupancy != 0UL)
            {
                int from = BoardOps.PopLsb(ref occupancy);
                if (Pieces.Pieces.SlidingPieces.Contains(type))
                    result |= MagicGenerator.GetAttacksForPiece(type, from, allPieces);
                else
                    result |= LeapingAttackTables.GetAttacksForPiece(type, from);
            }   
        }
        return result;
    }
}