#define DEBUG
namespace CBBL.src.Util;

public class PlatformHelper
{

#if DEBUG
    public static readonly string ProjectRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
#else
    public static readonly string ProjectRoot = Path.GetFullPath(AppContext.BaseDirectory);
#endif

}