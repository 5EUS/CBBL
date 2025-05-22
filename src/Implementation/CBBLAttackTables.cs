using CBBL.src.Interfaces;

namespace CBBL.src.Implementation;

public class CBBLAttackTables : IAttackTables
{
    public ILeapingAttackTables LeapingAttackTables { get; } = new CBBLLeapingAttackTables();
    public IMagicGenerator MagicGenerator { get; } = new CBBLMagicGenerator();
}