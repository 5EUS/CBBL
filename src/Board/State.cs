using CBBL.src.Interfaces;
using CBBL.src.Pieces;

namespace CBBL.src.Board;

/// <summary>
/// Available colors to play as
/// </summary>
public enum PlayerColor
{
    White,
    Black
}

/// <summary>
/// Useful state operations
/// </summary>
public static class State
{
    /// <summary>
    /// Convert a boolean to the corresponding player color
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static PlayerColor FromBool(bool color)
    {
        return color ? PlayerColor.White : PlayerColor.Black;
    }
}
