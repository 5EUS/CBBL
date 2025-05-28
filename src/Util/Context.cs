
namespace CBBL.src.Util;

public static class Context
{
    private static ServiceLoader? _instance;
    private static readonly Lock _lock = new();
    private static bool _initialized = false;

    public static ServiceLoader Instance
    {
        get
        {
            if (_instance == null)
                throw new InvalidOperationException("ServiceLoader is not initialized.");
            return _instance;
        }
    }

    public static void Register(ServiceLoader loader)
    {
        ArgumentNullException.ThrowIfNull(loader);
        lock (_lock)
        {
            if (_initialized)
                throw new InvalidOperationException("ServiceLoader is already registered.");
            _instance = loader;
            _initialized = true;
        }
    }
}