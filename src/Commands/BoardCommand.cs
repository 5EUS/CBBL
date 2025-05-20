namespace CBBL.src.Commands;

public abstract class BoardCommand(Board.Board board) : BaseCommand
{
    public Board.Board Board { get; set; } = board;
}