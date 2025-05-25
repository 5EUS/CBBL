using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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
    /// Get the bitboard of one square. Useful for aggregating bits in BB creation
    /// </summary>
    /// <param name="square">The square to occupy</param>
    /// <returns>A bitboard with a given square occupied</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong Bit(string square) => 1UL << BoardUtils.SquareToIndex(square);


    [DllImport("rmlib.so", EntryPoint = "count_1s", CallingConvention = CallingConvention.Cdecl)]
    public static extern int PopulationCount(ulong b);

    [DllImport("rmlib.so", EntryPoint = "surrounding", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong Surrounding(int square);
}