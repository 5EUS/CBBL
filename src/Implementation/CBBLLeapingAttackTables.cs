using CBBL.src.Board;
using CBBL.src.Debugging;
using CBBL.src.Interfaces;

namespace CBBL.src.Implementation;

public class CBBLLeapingAttackTables : ILeapingAttackTables
{
    public ulong[] KnightAttacks { get; } = new ulong[BoardGlobals.Instance.NumSquares];

    public ulong[] KingAttacks { get; } = new ulong[BoardGlobals.Instance.NumSquares];

    public ulong[] PawnAttacksWhite { get; } = new ulong[BoardGlobals.Instance.NumSquares];

    public ulong[] PawnAttacksBlack { get; } = new ulong[BoardGlobals.Instance.NumSquares];

    public List<ulong> Attacks { get; } = [];

    public CBBLLeapingAttackTables()
    {
        Logger.DualLogLine();
        Logger.DualLogLine("Initializing leaping piece attacks...");
        Logger.DualLogLine();
        for (int square = 0; square < BoardGlobals.Instance.NumSquares; square++)
        {
            KnightAttacks[square] = GenerateKnightAttacks(square);
            Logger.DualLogLine($"Knight at square {square} registered attacks {KnightAttacks[square]}");
            KingAttacks[square] = GenerateKingAttacks(square);
            Logger.DualLogLine($"King at square {square} registered attacks {KingAttacks[square]}");
            PawnAttacksWhite[square] = GeneratePawnAttacks(square, true);
            Logger.DualLogLine($"White pawn at square {square} registered attacks {PawnAttacksWhite[square]}");
            PawnAttacksBlack[square] = GeneratePawnAttacks(square, false);
            Logger.DualLogLine($"Black pawn at square {square} registered attacks {PawnAttacksBlack[square]}");

            Attacks.Add(KnightAttacks[square]);
            Attacks.Add(KingAttacks[square]);
            Attacks.Add(PawnAttacksWhite[square]);
            Attacks.Add(PawnAttacksBlack[square]);
        }
        Logger.DualLogLine();
        Logger.DualLogLine("Successfully initialized leaping piece attacks");
        Logger.DualLogLine();
    }

    public static ulong GenerateKnightAttacks(int square)
    {
        ulong attacks = 0UL;
        int rank = square / BoardGlobals.Instance.NumRanks;
        int file = square % BoardGlobals.Instance.NumFiles;

        int[] dRank = [-2, -1, 1, 2, 2, 1, -1, -2];
        int[] dFile = [1, 2, 2, 1, -1, -2, -2, -1];

        for (int i = 0; i < 8; i++)
        {
            int r = rank + dRank[i];
            int f = file + dFile[i];

            if (r >= 0 && r < BoardGlobals.Instance.NumRanks && f >= 0 && f < BoardGlobals.Instance.NumFiles)
            {
                int targetSquare = r * BoardGlobals.Instance.NumRanks + f;
                attacks |= 1UL << targetSquare;
            }
        }

        return attacks;
    }

    public static ulong GenerateKingAttacks(int square)
    {
        ulong attacks = 0UL;
        int rank = square / BoardGlobals.Instance.NumRanks;
        int file = square % BoardGlobals.Instance.NumFiles;

        for (int r = -1; r <= 1; r++)
        {
            for (int f = -1; f <= 1; f++)
            {
                if (r == 0 && f == 0) continue;

                int newRank = rank + r;
                int newFile = file + f;

                if (newRank >= 0 && newRank < BoardGlobals.Instance.NumRanks && newFile >= 0 && newFile < BoardGlobals.Instance.NumFiles)
                {
                    int targetSquare = newRank * BoardGlobals.Instance.NumRanks + newFile;
                    attacks |= 1UL << targetSquare;
                }
            }
        }

        return attacks;
    }

    public static ulong GeneratePawnAttacks(int square, bool isWhite)
    {
        ulong attacks = 0UL;
        int rank = square / BoardGlobals.Instance.NumRanks;
        int file = square % BoardGlobals.Instance.NumFiles;

        if (isWhite)
        {
            if (rank < BoardGlobals.Instance.NumRanks - 1)
            {
                if (file > 0) attacks |= 1UL << ((rank + 1) * BoardGlobals.Instance.NumRanks + (file - 1));
                if (file < BoardGlobals.Instance.NumFiles - 1) attacks |= 1UL << ((rank + 1) * BoardGlobals.Instance.NumRanks + file + 1);
            }
        }
        else
        {
            if (rank > 0)
            {
                if (file > 0) attacks |= 1UL << ((rank - 1) * BoardGlobals.Instance.NumRanks + (file - 1));
                if (file < BoardGlobals.Instance.NumFiles - 1) attacks |= 1UL << ((rank - 1) * BoardGlobals.Instance.NumRanks + file + 1);
            }
        }

        return attacks;
    } 
}