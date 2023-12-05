namespace Helios.Modbus.Internals;

public class HeliosParameter
{
    protected object? max;
    protected object? min;

    public HeliosParameter(string code, ushort registerCount, string description, AccessMode access,
        Type valueType, string? min = null, string? max = null)
    {
        Code = code;
        RegisterCount = registerCount;
        Description = description;
        Access = access;
        ValueType = valueType;

        SetMinMaxValues(valueType, min, max);
    }

    public string Code { get; }

    public ushort RegisterCount { get; }

    public string Description { get; }

    public AccessMode Access { get; }

    public Type ValueType { get; }

    private void SetMinMaxValues(Type valueType, string? min, string? max)
    {
        try
        {
            if (min != null)
            {
                var minValue = Convert.ChangeType(min, valueType);
                this.min = minValue;
            }
        }
        catch (Exception)
        {
            // no valid boundary?
        }

        try
        {
            if (max != null)
            {
                var maxValue = Convert.ChangeType(max, valueType);
                this.max = maxValue;
            }
        }
        catch (Exception)
        {
            // no valid boundary?
        }
    }
}

public class HeliosParameter<T> : HeliosParameter
{
    public HeliosParameter(string code, ushort registerCount, string description, AccessMode access, T? min = default,
        T? max = default)
        : base(code, registerCount, description, access, typeof(T))
    {
        this.min = min;
        this.max = max;
    }

    public T? Min => (T?)min;

    public T? Max => (T?)max;
}