using System.Collections.Concurrent;

public class DarwinLoggingProvider : ILoggerProvider
{
    private readonly IDisposable changeToken;
    private DarwinLoggingOptions options;
    private readonly ConcurrentDictionary<string, DarwinLogger> loggers = new ConcurrentDictionary<string, DarwinLogger>();
    public DarwinLoggingProvider(IOptionsMonitor<DarwinLoggingOptions> config)
    {
        this.options = config.CurrentValue;
        changeToken = config.OnChange(updatedConfig => 
        {
            this.options = updatedConfig;
            foreach(var logger in this.loggers)
            {
                logger.Value.UpdateOptions(this.options);
            }
        });
    }

    public ILogger CreateLogger(string categoryName)
    {
        if (!loggers.TryGetValue(categoryName, out var logger))
        {
            logger = new DarwinLogger(this.options, categoryName);
        }
        return logger;
    }

    public void Dispose()
    {
        foreach (var logger in loggers)
        {
            try
            {
                ((IDisposable)logger.Value).Dispose();
            }
            catch
            {
                // what do?
            }
        }
        loggers.Clear();
    }
}
