using System.Diagnostics;

public record DarwinLoggingOptions
{
    public string Subsystem { get; set; } = Process.GetCurrentProcess().ProcessName;

    public IDictionary<LogLevel, DarwinLogLevel> LogLevelMap { get; set; } = new Dictionary<LogLevel, DarwinLogLevel>
    {
        { LogLevel.Debug, DarwinLogLevel.Debug },
        { LogLevel.Trace, DarwinLogLevel.Debug },
        { LogLevel.Information, DarwinLogLevel.Info },
        { LogLevel.Warning, DarwinLogLevel.Error },
        { LogLevel.Error, DarwinLogLevel.Error },
        { LogLevel.Critical, DarwinLogLevel.Fault },
    };

    public DarwinLogLevel DefaultUnmappedLogLevel { get; set; } = DarwinLogLevel.Info;

    public LogLevel MinLogLevel { get; set; } = LogLevel.Information;
}