using CBBL.src.Abstraction;
using CBBL.src.Debugging;
using CBBL.src.Interfaces;

namespace CBBL.src.Commands;

/// <summary>
/// Registry for commands to use in the engine
/// </summary>
public class CommandRegistry : BitboardSingleton
{
    private readonly Dictionary<string, ICommand> _commands = [];
    private readonly Dictionary<string, ICommand> _aliases = [];

    private CommandRegistry()
    {
        Register(new HelpCommand());
        Register(new ViewBitboardCommand());
    }

    /// <summary>
    /// The global instance of commands
    /// </summary
    private static CommandRegistry? _instance;
    public static CommandRegistry Instance => _instance ??= new CommandRegistry();

    /// <summary>
    /// Register a given command to the instance
    /// </summary>
    /// <param name="command">Command to register</param>
    public void Register(ICommand command)
    {
        _commands[command.Name.ToLower()] = command;
        _aliases[command.Alias.ToLower()] = command;
    }

    /// <summary>
    /// Execute a given command
    /// </summary>
    /// <param name="input">Command arguments</param>
    /// <returns>The result of the command operation</returns>
    public bool Execute(string input)
    {
        string[] parts = input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0) return false;

        string name = parts[0].ToLower();
        string[] args = [.. parts.Skip(1)];

        if (_commands.TryGetValue(name, out var command) || _aliases.TryGetValue(name, out command))
        {
            command.Execute(args);
            return true;
        }

        Logger.LogToFileLine($"Tried to use command {name}", LogLevel.Warning);
        Logger.LogLine($"Unknown command: {name}", LogLevel.Warning);
        return false;
    }

    /// <summary>
    /// All registered commands
    /// </summary>
    public IEnumerable<ICommand> AllCommands => _commands.Values;
    
    public IEnumerable<ICommand> AllAliases => _aliases.Values;
}