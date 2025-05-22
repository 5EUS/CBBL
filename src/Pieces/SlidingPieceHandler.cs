using System.Reflection;
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

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate Result GetValueDelegate();

    [DllImport("rmlib.so", EntryPoint = "GetMagics", CallingConvention = CallingConvention.Cdecl)]
    public static extern Result GetMagics();

    public static class RMLib
    {
        public static IntPtr LoadNativeLibrary()
        {
            string libName = "rmlib";
            Assembly assembly = Assembly.GetExecutingAssembly();
            DllImportSearchPath searchFlags = DllImportSearchPath.AssemblyDirectory |
                                              DllImportSearchPath.SafeDirectories |
                                              DllImportSearchPath.UseDllDirectoryForDependencies;
            IntPtr libHandle = NativeLibrary.Load(libName, assembly, searchFlags);

            return libHandle;
        }
    }
}