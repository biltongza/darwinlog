# DarwinLog
Macintosh native `os_log` logger provider implementation for Microsoft.Extensions.Logging 

For more details on `os_log` see [Apple documentation](https://developer.apple.com/documentation/os/os_log).

## OS Support
I have only tested this on MacOS 12.0.1 on an M1 chip. YMMV.

## Usage

```
using Microsoft.Logging.Extensions;

Host.CreateDefaultBuilder(args)
    .ConfigureLogging(logging =>
    {
        logging.AddDarwinLog(configure =>
        {
            // set configuration here
        });
    });
```

## Configuration

The possible configuration options are:
| Parameter | Description |
| --- | --- |
| `Subsystem` | This parameter is passed to the underlying `os_log_create` method to identify your logs. Defaults to `Process.GetCurrentProces().ProcessName` |
| `LogLevelMap` | The log levels of `os_log` and those of `Microsoft.Extensions.Logging.LogLevel` do not match up 1:1, so they need to be mapped. See below for the default mapping. |
| `DefaultUnmappedLogLevel` | If the log level of the event is not present in `LogLevelMap`, this level is used instead. Defaults to `DarwinLogLevel.Info` |
| `MinLogLevel` | The minimum log level to log. Defaults to `LogLevel.Information` | 

Default `LogLevelMap`:

| `LogLevel` | `DarwinLogLevel` |
| --- | --- |
| `LogLevel.Debug` | `DarwinLogLevel.Debug` |
| `LogLevel.Trace` | `DarwinLogLevel.Debug` |
| `LogLevel.Information` | `DarwinLogLevel.Info` |
| `LogLevel.Warning` | `DarwinLogLevel.Error` |
| `LogLevel.Error` | `DarwinLogLevel.Error` |
| `LogLevel.Critical` | `DarwinLogLevel.Fault` |


