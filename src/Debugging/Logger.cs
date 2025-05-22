using CBBL.src.Util;

namespace CBBL.src.Debugging;

/// <summary>
/// Determines which level a given log message is
/// </summary>
public enum LogLevel
{
    None = 0,
    Verbose = 6,
    Error = 5,
    Warning = 3,
    Info = 2,
    Debug = 1,
    Test = 7
}

/// <summary>
/// A static logger used in all output for CBBL engines
/// </summary>
public static class Logger
{
    public static LogLevel CurrentLogLevel { get; private set; } = LogLevel.Info;
    public static bool DoLog => CurrentLogLevel != LogLevel.None;

    public static readonly string FolderPath = Path.Combine(PlatformHelper.ProjectRoot, "logs");
    public static readonly string FilePath = Path.Combine(FolderPath, "log.bin");
    private static StreamWriter? _writer;
    private static TextWriter _altOut = Console.Out;

    public static void Init()
    {
        if (!File.Exists(FolderPath))
            Directory.CreateDirectory(FolderPath);
        _writer = new(FilePath, true);
    }

    public static void SetLogLevel(LogLevel level)
    {
        CurrentLogLevel = level;
    }

    public static void LogToFile(string message, LogLevel level = LogLevel.Debug)
    {
        if (CurrentLogLevel == LogLevel.None)
            return;

        _writer?.Write(GetPrefix(level) + message);
    }

    public static void Log(string message, LogLevel level = LogLevel.Debug)
    {
        if (CurrentLogLevel == LogLevel.None)
            return;

        _altOut.Write(GetPrefix(level) + message);
    }

    public static void DualLog(string message, LogLevel level = LogLevel.Debug)
    {
        if (CurrentLogLevel == LogLevel.None)
            return;

        _altOut.Write(GetPrefix(level) + message);
        _writer?.Write(GetPrefix(level) + message);
    }

    private static string GetPrefix(LogLevel level)
    {
        if (level == LogLevel.None)
            return "";
        return $"[{level.ToString().ToUpper()}] ";
    }

    public static void LogToFileLine(string message = "", LogLevel level = LogLevel.Debug) => LogToFile(message + "\n", level);
    public static void LogLine(string message = "", LogLevel level = LogLevel.Debug) => Log(message + "\n", level);
    public static void DualLogLine(string message = "", LogLevel level = LogLevel.Debug) => DualLog(message + "\n", level);
}