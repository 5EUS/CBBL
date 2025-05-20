namespace CBBL.src.Interfaces;

public interface ICommand
{
    string Name { get; }
    string Alias { get;  }
    string Description { get; }
    string Usage { get; }
    void Execute(string[] args);
}