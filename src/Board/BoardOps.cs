using System.Runtime.CompilerServices;

namespace CBBL.src.Board;

/// <summary>
/// Useful constants and operations for working with bitboards
/// </summary>
public class BoardOps
{
    // File masks (vertical)
    public const ulong FILE_A = 0x0101010101010101UL;
    public const ulong FILE_B = 0x0202020202020202UL;
    public const ulong FILE_G = 0x4040404040404040UL;
    public const ulong FILE_H = 0x8080808080808080UL;

    // Rank masks (horizontal)
    public const ulong RANK_1 = 0x00000000000000FFUL;
    public const ulong RANK_2 = 0x000000000000FF00UL;
    public const ulong RANK_3 = 0x0000000000FF0000UL;
    public const ulong RANK_4 = 0x00000000FF000000UL;
    public const ulong RANK_5 = 0x000000FF00000000UL;
    public const ulong RANK_6 = 0x0000FF0000000000UL;
    public const ulong RANK_7 = 0x00FF000000000000UL;
    public const ulong RANK_8 = 0xFF00000000000000UL;

    // Shift functions
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong North(ulong b) => b << 8;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong South(ulong b) => b >> 8;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong East(ulong b) => (b & ~FILE_H) >> 1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong West(ulong b) => (b & ~FILE_A) << 1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong NorthEast(ulong b) => (b & ~FILE_H) << 7;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong NorthWest(ulong b) => (b & ~FILE_A) << 9;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong SouthEast(ulong b) => (b & ~FILE_H) >> 9;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong SouthWest(ulong b) => (b & ~FILE_A) >> 7;

    /// <summary>
    /// Mask for the surrounding squares
    /// </summary>
    /// <param name="square">The square to query</param>
    /// <returns>A mask of the surrounding squares</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong Surrounding(int square)
    {
        int rank = square / 8;
        int file = square % 8;

        ulong result = 0UL;

        for (int dr = -1; dr <= 1; dr++)
        {
            for (int df = -1; df <= 1; df++)
            {
                if (dr == 0 && df == 0)
                    continue;

                int r = rank + dr;
                int f = file + df;

                if (r >= 0 && r < 8 && f >= 0 && f < 8)
                {
                    result |= 1UL << (r * 8 + f);
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Count the number of occupied bits
    /// </summary>
    /// <param name="bitboard">The bitboard to query</param>
    /// <returns>The number of occupied bits</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CountBits(ulong bitboard)
    {
        int count = 0;
        while (bitboard != 0)
        {
            count++;
            bitboard &= bitboard - 1;
        }
        return count;
    }

    /// <summary>
    /// Get the bitboard of one square. Useful for aggregating bits in BB creation
    /// </summary>
    /// <param name="square">The square to occupy</param>
    /// <returns>A bitboard with a given square occupied</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong Bit(string square) => 1UL << BoardUtils.SquareToIndex(square);

    /// <summary>
    /// Gets the blocker permutation for the given index for the given premask
    /// </summary>
    /// <param name="index"></param>
    /// <param name="premask"></param>
    /// <returns></returns>
    public static ulong GetBlockerPermutation(int index, ulong premask)
    {
        ulong result = 0UL;

        List<int> validPositions = [];
        for (int i = 0; i < 64; i++)
        {
            if (((premask >> i) & 1UL) == 1UL)
            {
                validPositions.Add(i);
            }
        }

        for (int i = 0; i < validPositions.Count; i++)
        {
            int position = validPositions[i];

            if (((index >> i) & 1) == 1)
            {
                result |= 1UL << position;
            }
        }

        return result;
    }
}