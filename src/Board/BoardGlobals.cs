using CBBL.src.Abstraction;

namespace CBBL.src.Board;

/// <summary>
/// A singleton data instance for various key values. Designed to be mutable.
/// </summary>
public class BoardGlobals : BitboardSingleton
{
    /// <summary>
    /// The number of files the engine currently has
    /// </summary>
    public int NumFiles = 8;

    /// <summary>
    /// The number of ranks the engine currently has
    /// </summary>
    public int NumRanks = 8;

    /// <summary>
    /// The total number of squares the engine currently has
    /// </summary>
    public int NumSquares => NumFiles * NumRanks;

    /// <summary>
    /// Half the total number of squares the engine currently has
    /// </summary>
    public int HalfSquares => NumSquares / 2;

    /// <summary>
    /// The number of pieces registered in the engine
    /// </summary>
    public int NumPieces = 12;

    /// <summary>
    /// Half the number of pieces registered in the engine
    /// </summary>
    public int HalfPieces => NumPieces / 2;

    private BoardGlobals()
    {
    }

    /// <summary>
    /// Instance for important board data used in various functions
    /// </summary>
    private static BoardGlobals? _instance;
    public static BoardGlobals Instance => _instance ??= new BoardGlobals();
}