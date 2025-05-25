using CBBL.src.Interfaces;

namespace CBBL.src.Implementation;

public class CBBLAttackTables(int debug) : IAttackTables
{
    public ILeapingAttackTables LeapingAttackTables { get; } = new CBBLLeapingAttackTables();
    public IMagicGenerator MagicGenerator { get; } = new CBBLMagicGenerator(debug);
}