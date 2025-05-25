namespace CBBL.src.Interfaces;

public interface IPieceMagic
{
    ulong[][] AttackTable { get; }

    ulong[] Masks { get; }

    IMagicGenerator Generator { get; }

    void Init();

}