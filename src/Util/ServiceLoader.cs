using CBBL.src.Abstraction;

namespace CBBL.src.Util;

public abstract class CBBLServiceLoader : BitboardSingleton
{
    public static new CBBLServiceLoader Instance
    {
        get
        {
            _instance ??= new DefaultServiceLoader();
            return (CBBLServiceLoader)_instance;
        }
    }

    public abstract void Init();

    public class DefaultServiceLoader : CBBLServiceLoader
    {
        public override void Init()
        {
        }
    }
}