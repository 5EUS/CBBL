using System.Runtime.InteropServices;

namespace CBBL.src.Pieces;

public class SlidingPieceHandler
{
    [StructLayout(LayoutKind.Sequential)]
    public struct SquareResult
    {
        public ulong Magic;
        public int Shift;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Result
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public SquareResult[] RookResults;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public SquareResult[] BishopResults;
    }

    [DllImport("rmlib", EntryPoint = "GetMagics", CallingConvention = CallingConvention.Cdecl)]
    public static extern Result GetMagics();
}