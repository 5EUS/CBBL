using System.Reflection;
using System.Runtime.InteropServices;

namespace CBBL.src.Pieces;

public class SlidingPieceHandler
{

    [StructLayout(LayoutKind.Sequential)]
    public struct Result
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public ulong[] RookResults;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public ulong[] BishopResults;
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate Result GetValueDelegate();


    [DllImport("rmlib.so", EntryPoint = "GetMagics", CallingConvention = CallingConvention.Cdecl)]
    public static extern Result GetMagics(int debug);

    [DllImport("rmlib.so", EntryPoint = "transform", CallingConvention = CallingConvention.Cdecl)]
    public static extern int Transform(ulong blockers, ulong magic, int used);

    [DllImport("rmlib.so", EntryPoint = "index_to_ulong", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong IndexToPermutation(int index, int bits, ulong mask);

    [DllImport("rmlib.so", EntryPoint = "rmask", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong RookMask(int square);

    [DllImport("rmlib.so", EntryPoint = "bmask", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong BishopMask(int square);

    [DllImport("rmlib.so", EntryPoint = "ratt", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong RookAttack(int square, ulong blockers);

    [DllImport("rmlib.so", EntryPoint = "batt", CallingConvention = CallingConvention.Cdecl)]
    public static extern ulong BishopAttack(int square, ulong blockers);
}