using CBBL.src.Abstraction;
using CBBL.src.Debugging;
using CBBL.src.Interfaces;

namespace CBBL.src.Commands;

public class CommandRegistry : BitboardSingleton
{
    private readonly Dictionary<string, ICommand> _commands = [];

    public static new CommandRegistry Instance
    {
        get
        {
            _instance ??= new CommandRegistry();
            return (CommandRegistry)_instance;
        }
    }

    public void Register(ICommand command)
    {
        _commands[command.Name.ToLower()] = command;
    }

    public bool Execute(string input)
    {
        string[] parts = input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0) return false;

        string name = parts[0].ToLower();
        string[] args = parts.Skip(1).ToArray();

        if (_commands.TryGetValue(name, out var command))
        {
            command.Execute(args);
            return true;
        }

        Logger.LogToFileLine($"Tried to use command {name}", LogLevel.Warning);
        Logger.LogLine($"Unknown command: {name}", LogLevel.Warning);
        return false;
    }

    public IEnumerable<ICommand> AllCommands => _commands.Values;
}