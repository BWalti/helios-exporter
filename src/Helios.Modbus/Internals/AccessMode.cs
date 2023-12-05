namespace Helios.Modbus.Internals;

[Flags]
public enum AccessMode
{
    R = 1,
    W = 2,
    RW = 3
}