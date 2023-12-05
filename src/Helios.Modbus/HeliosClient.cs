using System.Buffers.Binary;
using System.Net.Sockets;
using System.Text;
using Helios.Modbus.Internals;
using Microsoft.Extensions.Options;
using NModbus;
using Serilog;

namespace Helios.Modbus;

public class HeliosClient : IDisposable
{
    private readonly TcpClient _client;
    private readonly IModbusMaster _modBus;

    public HeliosClient(IOptions<HeliosClientOptions> options, IModbusLogger? logAdapter = null)
    {
        var optionsValue = options.Value;
        _client = new TcpClient(optionsValue.Host, optionsValue.Port)
        {
            ReceiveTimeout = (int)optionsValue.Timeout.TotalMilliseconds,
            SendTimeout = (int)optionsValue.Timeout.TotalMilliseconds
        };

        _modBus = new ModbusFactory(logger: logAdapter)
            .CreateMaster(_client);
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    public async Task<T?> Query<T>(HeliosParameter<T> parameter)
    {
        Log.Debug("Querying {code}...", parameter.Code);

        if (!_client.Connected) return default;

        await InternalWrite(parameter.Code);
        return await InternalRead(parameter);
    }

    public async Task Write<T>(HeliosParameter<T> parameter, T value)
    {
        if (!parameter.Access.HasFlag(AccessMode.W))
            throw new InvalidOperationException(
                $"Cannot write parameter {parameter.Description} as it is not writable!");

        if (!_client.Connected) throw new InvalidOperationException("Client is not connected!");

        var ascii = $"{parameter.Code}={value}";

        Log.Information("Going to set: {parameterWithValue}", ascii);
        await InternalWrite(ascii);
    }

    private async Task<T?> InternalRead<T>(HeliosParameter<T> parameter)
    {
        var result = await _modBus.ReadHoldingRegistersAsync(
            HeliosDefaults.SlaveAddress,
            HeliosDefaults.Offset,
            parameter.RegisterCount);

        var decoded = ToAscii(result);
        Log.Debug("Decoded: {decoded}", decoded);

        if (TryExtractValue(parameter.Code, decoded, out var valueAsString))
        {
            Log.Debug("Trying to convert {value} to {type}", valueAsString, typeof(T).Name);
            return (T)Convert.ChangeType(valueAsString, typeof(T));
        }

        return default;
    }

    private async Task InternalWrite(string ascii)
    {
        var data = ToUShorts(ascii).ToArray();
        await _modBus.WriteMultipleRegistersAsync(HeliosDefaults.SlaveAddress, HeliosDefaults.Offset, data);
    }

    private static string ToAscii(Span<ushort> inputShorts)
    {
        var bytes = new Span<byte>(new byte[inputShorts.Length * 2]);

        for (var i = 0; i < inputShorts.Length; i++)
        {
            var targetWindow = bytes.Slice(i * 2, 2);
            BinaryPrimitives.WriteUInt16BigEndian(targetWindow, inputShorts[i]);
        }

        return Encoding.ASCII.GetString(bytes);
    }

    private static Span<ushort> ToUShorts(string ascii)
    {
        byte[] GetPaddedByteArray()
        {
            var bytes = Encoding.ASCII.GetBytes($"{ascii}\0");

            return bytes.Length % 2 == 1
                ? bytes.Concat(new byte[] { 0x0 }).ToArray()
                : bytes;
        }

        Span<byte> span = GetPaddedByteArray();
        var result = new Span<ushort>(new ushort[span.Length / 2]);

        for (var i = 0; i < span.Length / 2; i++)
        {
            var segment = span.Slice(i * 2, 2);
            result[i] = BinaryPrimitives.ReadUInt16BigEndian(segment);
        }

        return result;
    }

    private static bool TryExtractValue(string parameter, string decoded, out string value)
    {
        value = string.Empty;

        if (decoded.StartsWith(parameter + "="))
        {
            Log.Debug("Answer does match requested parameter: {parameter}", parameter);
            var startIndex = parameter.Length + 1;
            var indexOfNull = decoded.IndexOfAny(new[] { '\0', '?' }, startIndex);

            value = decoded.Substring(startIndex, indexOfNull - startIndex);
            Log.Debug("Value: {value}", value);

            return true;
        }

        Log.Warning("Answer {answer} does NOT match requested parameter: {parameter}", decoded, parameter);

        return false;
    }
}