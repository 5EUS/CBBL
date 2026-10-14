using CBBL.src.Board;
using CBBL.src.Debugging;

namespace CBBL.src.Commands;

public class IndexToSquareCommand : BaseCommand
{
    public override string Name => "indextosquare";

    public override string Alias => "its";

    public override string Description => "Convert an integer index into row/column notation";

    public override string Usage => "indextosquare <int>";

    public override int MinArgs => 1;

    public override int MaxArgs => 1;

    public override bool Execute(string[] args)
    {
        if (!CheckUsage(args.Length))
            return false;

        if (!int.TryParse(args[0], out var index))
        {
            Logger.LogLine("Invalid index.", LogLevel.Warning);
            return false;
        }

        Logger.LogLine($"{BoardUtils.IndexToSquare(index)}");
        return true;    
    }
}