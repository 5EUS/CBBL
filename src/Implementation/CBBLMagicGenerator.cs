using CBBL.src.Board;
using CBBL.src.Debugging;
using CBBL.src.Interfaces;
using static CBBL.src.Pieces.SlidingPieceHandler;

namespace CBBL.src.Implementation;

public class CBBLMagicGenerator : IMagicGenerator
{
    public Result Magics { get; private set; }
    public RookMagics RookMagics { get; private set; }
    public BishopMagics BishopMagics { get; private set; }

    public CBBLMagicGenerator(int debug)
    {
        Logger.DualLogLine("Registering sliding piece magics...");
        Magics = GetMagics(debug);
        Logger.DualLogLine("Successfully registered sliding piece magics");

        Logger.DualLogLine();

        Logger.DualLogLine("Creating sliding piece attack tables...");
        RookMagics = new(this);
        BishopMagics = new(this);
        Logger.DualLogLine("Successfully created sliding piece attack tables");
    }

    public ulong GetRookAttacks(int square, ulong blockers)
    {
        ulong mask = RookMask(square); // precompute
        ulong relevant = mask & blockers;
        int shift = 64 - BoardOps.PopulationCount(mask); // pre compute
        ulong squareResult = Magics.RookResults[square];
        int index = (int)((relevant * squareResult) >> shift);
        return RookMagics.AttackTable[square][index];
    }

    public ulong GetBishopAttacks(int square, ulong blockers)
    {
        ulong mask = BishopMask(square);
        ulong relevant = mask & blockers;
        int shift = 64 - BoardOps.PopulationCount(mask);
        ulong squareResult = Magics.BishopResults[square];
        int index = (int)((relevant * squareResult) >> shift);
        return BishopMagics.AttackTable[square][index];
    }
}