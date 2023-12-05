using NModbus;
using Serilog;
using Serilog.Events;

namespace Helios.Modbus.Internals;

public class SerilogAdapter : IModbusLogger
{
    private readonly ILogger _log = Serilog.Log.Logger.ForContext<SerilogAdapter>();

    public void Log(LoggingLevel level, string message)
    {
        switch (level)
        {
            case LoggingLevel.Critical:
                _log.Fatal(message);
                break;

            case LoggingLevel.Debug:
                _log.Debug(message);
                break;

            case LoggingLevel.Error:
                _log.Error(message);
                break;

            case LoggingLevel.Information:
                _log.Information(message);
                break;

            case LoggingLevel.Trace:
                _log.Verbose(message);
                break;

            case LoggingLevel.Warning:
                _log.Warning(message);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(level), level, null);
        }
    }

    public bool ShouldLog(LoggingLevel level)
    {
        return _log.IsEnabled(ToLogEventLevel(level));
    }

    private static LogEventLevel ToLogEventLevel(LoggingLevel level)
    {
        return level switch
        {
            LoggingLevel.Critical => LogEventLevel.Fatal,
            LoggingLevel.Debug => LogEventLevel.Debug,
            LoggingLevel.Error => LogEventLevel.Error,
            LoggingLevel.Information => LogEventLevel.Information,
            LoggingLevel.Trace => LogEventLevel.Verbose,
            LoggingLevel.Warning => LogEventLevel.Warning,
            _ => throw new ArgumentOutOfRangeException(nameof(level), level, null)
        };
    }
}