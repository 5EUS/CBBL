using CBBL.src.Board;
using CBBL.src.Debugging;
using CBBL.src.Interfaces;
using CBBL.src.Pieces;
using static CBBL.src.Pieces.SlidingPieceHandler;

namespace CBBL.src.Implementation.AttackTables.Magic;

public class CBBLMagicGenerator : IMagicGenerator
{
    internal Result Magics { get; private set; }
    internal CBBLRookMagics RookMagics { get; private set; }
    internal CBBLBishopMagics BishopMagics { get; private set; }

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

    public ulong GetQueenAttacks(int square, ulong blockers)
    {
        return GetRookAttacks(square, blockers) | GetBishopAttacks(square, blockers);
    }

    public ulong GetRookMagic(int square)
    {
        return Magics.RookResults[square];
    }

    public ulong GetBishopMagic(int square)
    {
        return Magics.BishopResults[square];
    }

    public ulong GetAttacksForPiece(PieceType pieceType, int square, ulong blockers)
    {
        return pieceType switch
        {
            PieceType.WhiteRook => GetRookAttacks(square, blockers),
            PieceType.BlackRook => GetRookAttacks(square, blockers),
            PieceType.WhiteBishop => GetBishopAttacks(square, blockers),
            PieceType.BlackBishop => GetBishopAttacks(square, blockers),
            PieceType.WhiteQueen => GetQueenAttacks(square, blockers),
            PieceType.BlackQueen => GetQueenAttacks(square, blockers),
            _ => throw new ArgumentException($"Invalid piece type: {pieceType}")
        };
    }
}