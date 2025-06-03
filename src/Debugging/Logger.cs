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
    Warning = 4,
    Command = 3,
    Info = 2,
    Debug = 1,
    Test = 7
}

/// <summary>
/// A static logger used in all output for CBBL engines
/// </summary>
public static class Logger
{
    public static LogLevel FileLogLevel { get; private set; } = LogLevel.Info;
    public static LogLevel AltLogLevel { get; private set; } = LogLevel.Info;
    public static bool DoFileLog => FileLogLevel != LogLevel.None;
    public static bool DoAltLog => AltLogLevel != LogLevel.None;

    public static readonly string FolderPath = Path.Combine(PlatformHelper.ProjectRoot, "logs");
    public static readonly string FilePath = Path.Combine(FolderPath, "log.bin");
    public static StreamWriter? Writer { get; private set; }
    public static TextWriter AltOut { get; private set; } = Console.Out;

    public static void Init()
    {
        if (!File.Exists(FolderPath))
            Directory.CreateDirectory(FolderPath);

        if (File.Exists(FilePath))
        {
            using FileStream fs = new(FilePath, FileMode.Open, FileAccess.Read);
            using StreamReader reader = new(fs);

            string? line = reader.ReadLine();

            if (line != null && long.TryParse(line, out long fileTimeUtc))
            {
                var dateTimeUtc = DateTime.FromFileTimeUtc(fileTimeUtc).ToUniversalTime().ToString();
                dateTimeUtc = dateTimeUtc.Replace('/', '_');
                dateTimeUtc = dateTimeUtc.Replace(' ', '_');
                var first = FilePath[..FilePath.LastIndexOf('.')];
                string ext = ".bin";
                File.Move(FilePath, first + "_" + dateTimeUtc + ext);
            }
            else
            {
                DualLogLine("Failed to parse the date of the latest log file.");
            }

            fs.Close();
            reader.Close();
        }

        Writer = new(FilePath, false);
        Writer.WriteLine(DateTime.Now.ToFileTimeUtc());
    }

    public static void SetFileLogLevel(LogLevel level)
    {
        FileLogLevel = level;
    }

    public static void SetAltLogLevel(LogLevel level)
    {
        AltLogLevel = level;
    }

    public static void LogToFile(string message, LogLevel level = LogLevel.Debug)
    {
        if (FileLogLevel == LogLevel.None && level !> LogLevel.Info)
            return;

        Writer?.Write(GetPrefix(level) + message);
    }

    public static void Log(string message, LogLevel level = LogLevel.Debug)
    {
        if (AltLogLevel == LogLevel.None && level !> LogLevel.Info)
            return;

        AltOut.Write(GetPrefix(level) + message);
    }

    public static void DualLog(string message, LogLevel level = LogLevel.Debug)
    {
        if (AltLogLevel != LogLevel.None || level > LogLevel.Info)
            AltOut.Write(GetPrefix(level) + message);

        if (FileLogLevel != LogLevel.None || level > LogLevel.Info)
            Writer?.Write(GetPrefix(level) + message);
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