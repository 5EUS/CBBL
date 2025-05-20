namespace CBBL.src.Abstraction;

public abstract class BitboardSingleton
{
    protected static BitboardSingleton? _instance;

    protected BitboardSingleton()
    {
    }

    public static BitboardSingleton Instance
    {
        get
        {
            _instance ??= new BitboardSingletonImpl();
            return _instance;
        }
    }

    private class BitboardSingletonImpl : BitboardSingleton
    {
    }
}