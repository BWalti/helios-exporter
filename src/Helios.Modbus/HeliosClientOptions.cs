using Helios.Modbus.Internals;

namespace Helios.Modbus;

public class HeliosClientOptions
{
    public string Host { get; set; } = "helios";
    public int Port { get; set; } = HeliosDefaults.Port;
    public TimeSpan Timeout { get; set; }
}