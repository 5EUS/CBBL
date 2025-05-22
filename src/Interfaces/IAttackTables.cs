namespace CBBL.src.Interfaces;

public interface IAttackTables
{
    ILeapingAttackTables LeapingAttackTables { get; }
    IMagicGenerator MagicGenerator { get; }
}