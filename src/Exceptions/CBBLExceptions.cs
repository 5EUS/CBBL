using CBBL.src.Debugging;

namespace CBBL.src.Exceptions;

public class UninitializedContextException : Exception
{
    public UninitializedContextException() : base("Context is not initialized") { }

    public UninitializedContextException(string message) : base(message) { }

    public UninitializedContextException(string message, Exception inner)
        : base(message, inner) { }
        
}

public class MagicCollisionException : Exception
{
    public MagicCollisionException() : base("Collision detected")
        { Logger.DualLogLine("Collision detected"); }

    public MagicCollisionException(string message) : base("Collision detected at " + message)
        { Logger.DualLogLine("Collision detected at " + message); }

    public MagicCollisionException(string message, Exception inner) : base("Collision detected at " + message, inner)
        { Logger.DualLogLine("Collision detected at " + message); }
}