using CBBL.src.Board;
using CBBL.src.Debugging;
using CBBL.src.Exceptions;
using CBBL.src.Interfaces;
using CBBL.src.Pieces;

namespace CBBL.src.Implementation.AttackTables.Magic;

public class CBBLBishopMagics : IPieceMagic
{
    public ulong[][] AttackTable { get; }

    public ulong[] Masks { get; }

    public IMagicGenerator Generator { get; }

    public CBBLBishopMagics(IMagicGenerator generator)
    {
        Generator = generator;
        Masks = new ulong[BoardGlobals.Instance.NumSquares];
        AttackTable = new ulong[BoardGlobals.Instance.NumSquares][];
        for (int i = 0; i < BoardGlobals.Instance.NumSquares; i++)
        {
            AttackTable[i] = new ulong[BoardGlobals.Instance.NumSquares ^ 2];
        }
        Init();
    }

    public void Init()
    {
        Logger.DualLogLine();
        Logger.DualLogLine("Initializing Bishop attack table...");
        Logger.DualLogLine();
        for (int square = 0; square < BoardGlobals.Instance.NumSquares; square++)
        {
            ulong mask = SlidingPieceHandler.BishopMask(square);
            int relevantBits = BoardOps.PopulationCount(mask);
            int tableSize = 1 << relevantBits;
            var squareResult = Generator.GetBishopMagic(square);
            Masks[square] = mask;
            AttackTable[square] = new ulong[tableSize];

            for (int perm = 0; perm < tableSize; perm++)
            {
                ulong blockers = SlidingPieceHandler.IndexToPermutation(perm, relevantBits, mask);
                Logger.DualLogLine($"Blocker {perm} at square {square}: {blockers:X16}");

                int index = SlidingPieceHandler.Transform(blockers, squareResult, relevantBits);

                ulong attack = SlidingPieceHandler.BishopAttack(square, blockers);
                Logger.DualLogLine($"Attack perm {perm} at square {square} placed in index {index}: {attack:X16}");

                if (AttackTable[square][index] != 0)
                    throw new MagicCollisionException(index.ToString());

                AttackTable[square][index] = attack;
            }
        }
        Logger.DualLogLine();
        Logger.DualLogLine("Bishop attack table initialized.");
        Logger.DualLogLine();
    }
}