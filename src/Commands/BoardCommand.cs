using CBBL.src.Interfaces;

namespace CBBL.src.Commands;

/// <summary>
/// An abstraction layer for commands used in mutating or viewing the board state
/// </summary>
/// <param name="board"></param>
public abstract class BoardCommand(IBoard board) : BaseCommand
{
    public IBoard Board { get; set; } = board;
}