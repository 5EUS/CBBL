using CBBL.src.Abstraction;

namespace CBBL.src.Board;

public class BoardGlobals : BitboardSingleton
{
    public int NumFiles = 8;
    public int NumRanks = 8;
    public int NumSquares => NumFiles * NumRanks;
    public int HalfSquares => NumSquares / 2;

    public int NumPieces = 12;
    public int HalfPieces => NumPieces / 2;

    public static new BoardGlobals Instance
    {
        get
        {
            _instance ??= new BoardGlobals();
            return (BoardGlobals)_instance;
        }
    }
}