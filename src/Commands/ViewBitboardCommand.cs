using CBBL.src.Board;
using CBBL.src.Debugging;

namespace CBBL.src.Commands;

public class ViewBitboardCommand : BaseCommand
{
    public override string Name => "viewbitboard";

    public override string Alias => "vbb";

    public override string Description => "Displays the given (in hex or dec) bitboard.";

    public override string Usage => "viewbitboard <bitboard>";

    public override int MinArgs => 1;

    public override int MaxArgs => 1;

    public override bool Execute(string[] args)
    {
        if (!CheckUsage(args.Length))
        {
            return false;
        }

        string input = args[0];
        ulong bitboard;

        if (input.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
        {
            if (!ulong.TryParse(input[2..], System.Globalization.NumberStyles.HexNumber, null, out bitboard))
            {
                Logger.LogLine($"Invalid hexadecimal bitboard: {input}", LogLevel.Warning);
                return false;
            }
        }
        else
        {
            if (!ulong.TryParse(input, out bitboard))
            {
                Logger.LogLine($"Invalid decimal bitboard: {input}", LogLevel.Warning);
                return false;
            }
        }

        Logger.LogLine($"Bitboard: {bitboard:X16} (Hex) | {bitboard} (Dec)\n{BoardUtils.GetBitboardString(bitboard)}", LogLevel.Command);
        return true;
    }
}