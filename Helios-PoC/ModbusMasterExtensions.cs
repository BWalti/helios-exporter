namespace Helios_PoC
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using NModbus;

    using Serilog;

    public static class ModbusMasterExtensions
    {
        public static async Task<T> QueryHeliosValue<T>(this IModbusMaster modbus, VariableDeclaration<T> parameter)
        {
            Log.Debug($"Querying {parameter.Code}...");
            var bytes = Encoding.ASCII.GetBytes($"{parameter.Code}\0");
            var ushorts = ToUShortArray(bytes);

            await modbus.WriteMultipleRegistersAsync(HeliosDefaults.SlaveAddress, HeliosDefaults.Offset, ushorts);
            var result =
                await modbus.ReadHoldingRegistersAsync(HeliosDefaults.SlaveAddress,
                                                       HeliosDefaults.Offset,
                                                       parameter.RegisterCount);

            bytes = FromShortArray(result);
            var decoded = Encoding.ASCII.GetString(bytes);

            Log.Debug($"Decoded: {decoded}");

            if (TryExtractValue(parameter.Code, decoded, out var value))
            {
                Log.Debug($"Trying to convert {value} to {typeof(T)}");
                return (T)Convert.ChangeType(value, typeof(T));
            }

            return default;
        }

        private static ushort[] ToUShortArray(IEnumerable<byte> input)
        {
            var array = input.ToArray();
            if (array.Length % 2 == 1)
            {
                array = array.Concat(new byte[] { 0x0 })
                             .ToArray();
            }

            var values = new List<ushort>();
            for (var i = 0; i < array.Length; i += 2)
            {
                var part = array.Skip(i)
                                .Take(2)
                                .ToArray();

                // not quite sure why I currently need to do this:
                part = part.Swap();

                values.Add(BitConverter.ToUInt16(part));
            }

            return values.ToArray();
        }

        private static bool TryExtractValue(string parameter, string decoded, out string value)
        {
            value = string.Empty;

            if (decoded.StartsWith(parameter + "="))
            {
                Log.Debug($"Answer does match requested parameter: {parameter}");
                var startIndex = parameter.Length + 1;
                var indexOfNull = decoded.IndexOfAny(new[] { '\0', '?' }, startIndex);
                Log.Debug($"Start: {startIndex}, End: {indexOfNull}");

                value = decoded.Substring(startIndex, indexOfNull - startIndex);
                Log.Debug($"Value: {value}");

                return true;
            }

            return false;
        }

        private static byte[] FromShortArray(ushort[] shorts)
        {
            return shorts.SelectMany(s =>
                         {
                             // not quite sure why I currently need to swap:
                             return BitConverter.GetBytes(s)
                                                .Swap();
                         })
                         .ToArray();
        }
    }
}