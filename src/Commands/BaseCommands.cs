using CBBL.src.Debugging;
using CBBL.src.Interfaces;

namespace CBBL.src.Commands;

public abstract class BaseCommand : ICommand
{
    public abstract string Name { get; }
    public abstract string Alias { get; }
    public abstract string Description { get; }
    public abstract string Usage { get; }
    public abstract int MinArgs { get; }
    public abstract int MaxArgs { get; }
    public abstract void Execute(string[] args);
    public virtual bool CheckUsage(int length)
    {
        if (length < MinArgs || length > MaxArgs)
        {
            Logger.LogToFileLine($"Command {Name} used incorrectly", LogLevel.Warning);
            Logger.DualLogLine($"Usage: {Name} => {Usage}", LogLevel.Info);
            return false;
        }
        return true;
    }
}