using CBBL.src.Board;
using CBBL.src.Debugging;

namespace CBBL.src.Commands;

public class SquareToIndexCommand : BaseCommand
{
    public override string Name => "squaretoindex";

    public override string Alias => "sti";

    public override string Description => "Convert a row/column notation square to an integer index";

    public override string Usage => "squaretoindex <a-h1-8>";

    public override int MinArgs => 1;

    public override int MaxArgs => 1;

    public override bool Execute(string[] args)
    {
        if (!CheckUsage(args.Length))
            return false;

        int index = BoardUtils.SquareToIndex(args[0]);
        if (index == -1)
            return false;

        Logger.LogLine($"{index}");            

        return true;    
    }
}