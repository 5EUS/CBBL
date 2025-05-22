
using CBBL.src.Implementation;
using CBBL.src.Util;

namespace CBBL.Tests;

public class UnitTest1
{
    [Fact]
    public void Test1()
    {
        var context = ServiceLoader.Instance;
        var board = new CBBLBoard();
        context.Init(board);
    }
}
