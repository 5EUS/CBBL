namespace CBBL.src.Board;

public enum PlayerColor
{
    White,
    Black
}

public static class State
{
    public static PlayerColor FromBool(bool color)
    {
        return color ? PlayerColor.White : PlayerColor.Black;
    }
}