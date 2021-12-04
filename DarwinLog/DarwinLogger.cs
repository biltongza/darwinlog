using System.Runtime.InteropServices;
using os_log_t = System.IntPtr;

public class DarwinLogger : ILogger, IDisposable
{
    internal os_log_t instance;
    private static readonly object instanceLock = new object();
    private DarwinLoggingOptions options;
    private readonly string category;

    public DarwinLogger(DarwinLoggingOptions options, string category)
    {
        this.options = options;
        this.category = category;
        this.instance = os_log_create(options.Subsystem, category);
    }

    void IDisposable.Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~DarwinLogger()
    {
        Dispose(false);
    }

    internal void UpdateOptions(DarwinLoggingOptions options)
    {
        lock (instanceLock)
        {
            this.options = options;
            Marshal.FreeHGlobal(this.instance);
            this.instance = os_log_create(options.Subsystem, this.category);
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (this.instance != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(instance);
            this.instance = IntPtr.Zero;
        }
    }

    [DllImport("libc")]
    static extern os_log_t os_log_create(string subsystem, string category);

    [DllImport("libdarwinlog", EntryPoint = "Log")]
    static extern void os_log(os_log_t log, string message);
    [DllImport("libdarwinlog", EntryPoint = "LogInfo")]
    static extern void os_log_info(os_log_t log, string message);
    [DllImport("libdarwinlog", EntryPoint = "LogDebug")]
    static extern void os_log_debug(os_log_t log, string message);
    [DllImport("libdarwinlog", EntryPoint = "LogError")]
    static extern void os_log_error(os_log_t log, string message);
    [DllImport("libdarwinlog", EntryPoint = "LogFault")]
    static extern void os_log_fault(os_log_t log, string message);
    public IDisposable BeginScope<TState>(TState state) => default!;

    public bool IsEnabled(LogLevel logLevel)
    {
        return logLevel >= this.options.MinLogLevel;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }
        
        if (formatter == null)
        {
            throw new ArgumentNullException(nameof(formatter));
        }
        
        var message = formatter(state, exception);
        
        lock (instanceLock)
        {
            if (!this.options.LogLevelMap.TryGetValue(logLevel, out var darwinLogLevel))
            {
                darwinLogLevel = this.options.DefaultUnmappedLogLevel;
            }
            
            switch (darwinLogLevel)
            {
                case DarwinLogLevel.Debug:
                    os_log_error(this.instance, message);
                    break;
                case DarwinLogLevel.Info:
                    os_log_info(this.instance, message);
                    break;
                case DarwinLogLevel.Error:
                    os_log_error(this.instance, message);
                    break;
                case DarwinLogLevel.Fault:
                    os_log_error(this.instance, message);
                    break;
            }
        }
    }
}