using System.Numerics;
using CBBL.src.Pieces;

namespace CBBL.src.Interfaces;


public enum MoveType
{
    LEGAL,
    PSEUDO_LEGAL,
    EVASION
}

public interface IMoveGenerator
{
    IPieceMoveGenerator Generator { get; }

    IEnumerable<Move> GenerateMoves(MoveType moveType);

}