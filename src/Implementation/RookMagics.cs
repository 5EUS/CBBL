using CBBL.src.Board;
using CBBL.src.Debugging;
using CBBL.src.Exceptions;
using CBBL.src.Interfaces;

namespace CBBL.src.Implementation;

public class RookMagics : IPieceMagic
{
    public ulong[][] AttackTable { get; }

    public ulong[] Masks { get; }

    public IMagicGenerator Generator { get; }

    public RookMagics(IMagicGenerator generator)
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

    public ulong GenerateAttack(int square, ulong blockers)
    {
        ulong attacks = 0UL;
        int rank = square / BoardGlobals.Instance.NumRanks;
        int file = square % BoardGlobals.Instance.NumFiles;

        for (int r = rank + 1; r <= BoardGlobals.Instance.NumRanks - 1; r++)
        {
            int sq = r * BoardGlobals.Instance.NumRanks + file;
            attacks |= 1UL << sq;
            if (((1UL << sq) & blockers) != 0) break;
        }

        for (int r = rank - 1; r >= 0; r--)
        {
            int sq = r * BoardGlobals.Instance.NumRanks + file;
            attacks |= 1UL << sq;
            if (((1UL << sq) & blockers) != 0) break;
        }

        for (int f = file - 1; f >= 0; f--)
        {
            int sq = rank * BoardGlobals.Instance.NumRanks + f;
            attacks |= 1UL << sq;
            if (((1UL << sq) & blockers) != 0) break;
        }

        for (int f = file + 1; f <= BoardGlobals.Instance.NumFiles; f++)
        {
            int sq = rank * BoardGlobals.Instance.NumRanks + f;
            attacks |= 1UL << sq;
            if (((1UL << sq) & blockers) != 0) break;
        }

        return attacks;
    }

    public ulong GeneratePremask(int square)
    {
        int rank = square / BoardGlobals.Instance.NumRanks;
        int file = square % BoardGlobals.Instance.NumFiles;

        ulong mask = 0;

        for (int r = rank + 1; r < BoardGlobals.Instance.NumRanks - 1; r++) 
            mask |= 1UL << (r * BoardGlobals.Instance.NumRanks + file);

        for (int r = rank - 1; r > 0; r--)
            mask |= 1UL << (r * BoardGlobals.Instance.NumRanks + file);

        for (int f = file + 1; f < BoardGlobals.Instance.NumFiles - 1; f++) 
            mask |= 1UL << (rank * BoardGlobals.Instance.NumRanks + f);

        for (int f = file - 1; f > 0; f--)
            mask |= 1UL << (rank * BoardGlobals.Instance.NumRanks + f);

        return mask;
    }

    public void Init()
    {
        Logger.DualLogLine();
        Logger.DualLogLine("Initializing Rook attack table...");
        Logger.DualLogLine();
        for (int square = 0; square < BoardGlobals.Instance.NumSquares; square++)
        {
            ulong mask = GeneratePremask(square); 
            int relevantBits = BoardOps.CountBits(mask);
            int tableSize = 1 << relevantBits;
            var squareResult = Generator.Magics.RookResults[square];
            Masks[square] = mask;
            AttackTable[square] = new ulong[tableSize];

            for (int perm = 0; perm < tableSize; perm++)
            {
                ulong blockers = BoardOps.GetBlockerPermutation(perm, mask);
                Logger.DualLogLine($"Blocker {perm} at square {square}: {blockers:X16}");

                int index = (int)((blockers * squareResult.Magic) >> squareResult.Shift);

                ulong attack = GenerateAttack(square, blockers);
                Logger.DualLogLine($"Attack perm {perm} at square {square} placed in index {index}: {attack:X16}");

                if (AttackTable[square][index] != 0)
                    throw new MagicCollisionException(index.ToString());

                AttackTable[square][index] = attack;
            }
        }
        Logger.DualLogLine();
        Logger.DualLogLine("Rook attack table initialized.");
        Logger.DualLogLine();
    }
}