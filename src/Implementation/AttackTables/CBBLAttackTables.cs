using CBBL.src.Board;
using CBBL.src.Implementation.AttackTables.Magic;
using CBBL.src.Interfaces;
using CBBL.src.Pieces;

namespace CBBL.src.Implementation.AttackTables;

public class CBBLAttackTables(int debug) : IAttackTables
{
    public ILeapingAttackTables LeapingAttackTables { get; } = new CBBLLeapingAttackTables();
    public IMagicGenerator MagicGenerator { get; } = new CBBLMagicGenerator(debug);


    public ulong Aggregate(BoardState boardState, PieceType[] types)
    {
        ulong result = 0UL;

        foreach (var type in types)
        {
            var occupancy = boardState.GetBitboardFor(type);
            while (occupancy != 0UL)
            {
                int from = BoardOps.PopLsb(ref occupancy);
                if (Pieces.Pieces.SlidingPieces.Contains(type))
                    result |= MagicGenerator.GetAttacksForPiece(type, from, BoardUtils.AllPieces(boardState));
                else
                    result |= LeapingAttackTables.GetAttacksForPiece(type, from);
            }   
        }

        return result;
    }
}