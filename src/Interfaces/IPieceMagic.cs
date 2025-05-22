namespace CBBL.src.Interfaces;

public interface IPieceMagic
{
    ulong[][] AttackTable { get; }

    ulong[] Masks { get; }

    IMagicGenerator Generator { get; }

    void Init();

    ulong GeneratePremask(int square);

    ulong GenerateAttack(int square, ulong blockers);

}