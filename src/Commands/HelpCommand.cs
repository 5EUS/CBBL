using CBBL.src.Debugging;

namespace CBBL.src.Commands;

public class HelpCommand : BaseCommand
{
    public override string Name => "help";

    public override string Alias => "h";

    public override string Description => "Displays help information for commands.";

    public override string Usage => "help [command]";

    public override int MinArgs => 0;

    public override int MaxArgs => 1;

    public override bool Execute(string[] args)
    {
        if (!CheckUsage(args.Length))
        {
            return false;
        }

        if (args.Length == 0)
        {
            Logger.LogLine("Available commands:", LogLevel.Command);
            foreach (var command in CommandRegistry.Instance.AllCommands)
            {
                Logger.LogLine($"{command.Name}: {command.Description}: {command.Usage}", LogLevel.Command);
            }
            return true;
        }
        else if (args.Length == 1)
        {
            string commandName = args[0].ToLower();
            if (CommandRegistry.Instance.AllCommands.FirstOrDefault(
                c => c.Name.Equals(commandName, StringComparison.CurrentCultureIgnoreCase)) is { } command)
            {
                Logger.LogLine($"Command: {command.Name}", LogLevel.Command);
                Logger.LogLine($"Description: {command.Description}", LogLevel.Command);
                Logger.LogLine($"Usage: {command.Usage}", LogLevel.Command);
                return true;
            }
            else
            {
                Logger.LogLine($"No command found with name '{commandName}'.", LogLevel.Warning);
                return false;
            }
        }
        return false;
    }
}