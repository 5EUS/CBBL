using CBBL.src.Debugging;
using CBBL.src.Interfaces;
using static CBBL.src.Pieces.SlidingPieceHandler;

namespace CBBL.src.Implementation;

public class CBBLMagicGenerator : IMagicGenerator
{
    public Result Magics { get; private set; }
    public RookMagics RookMagics { get; private set; }
    public BishopMagics BishopMagics { get; private set; }

    public CBBLMagicGenerator()
    {
        Logger.DualLogLine("Registering sliding piece magics...");
        Magics = GetMagics();
        Logger.DualLogLine("Successfully registered sliding piece magics");

        Logger.DualLogLine("Creating sliding piece attack tables...");
        RookMagics = new(this);
        BishopMagics = new(this);
        Logger.DualLogLine("Successfully created sliding piece attack tables");
    }
}