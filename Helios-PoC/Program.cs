namespace Helios_PoC
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Configuration;
    using NModbus;
    using Serilog;

    public class Program
    {
        private const int Offset = 1;
        private const byte SlaveAddress = 180;
        private const int Port = 502;
        private const int TimeoutMs = 2000;

        // see reference: https://www.easycontrols.net/en/service/downloads/send/4-software/16-modbus-dokumentation-f%C3%BCr-kwl-easycontrols-ger%C3%A4te
        private const string Aussenluft = "v00104";
        private const string Zuluft = "v00105";
        private const string Fortluft = "v00106";
        private const string Abluft = "v00107";
        private static readonly IPAddress IpAddress = IPAddress.Parse("192.168.0.228");

        public static async Task Main(string[] args)
        {
            ConfigureServices();

            do
            {
                Log.Information("Querying temperatures...");

                var aussenluft = await QueryHeliosValue(Aussenluft);
                var zuluft = await QueryHeliosValue(Zuluft);
                var fortluft = await QueryHeliosValue(Fortluft);
                var abluft = await QueryHeliosValue(Abluft);

                Log.Information($"Results: {aussenluft} / {zuluft} / {fortluft} / {abluft}");
            } while (Console.ReadKey().Key == ConsoleKey.F5);

            Log.CloseAndFlush();
        }

        private static void ConfigureServices()
        {
            // Read the application settings file.
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            // Setting up the static Serilog logger.
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            // Set the default culture.
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
        }

        private static async Task<string> QueryHeliosValue(string parameter)
        {
            Log.Debug($"Querying {parameter}...");
            var bytes = Encoding.ASCII.GetBytes($"{parameter}\0");

            var ushorts = ToUShortArray(bytes);

            using (var client = new TcpClient())
            {
                client.ReceiveTimeout = TimeoutMs;
                client.SendTimeout = TimeoutMs;
                client.Client.Connect(IpAddress, Port);

                if (client.Connected)
                {
                    var factory = new ModbusFactory();
                    var modbus = factory.CreateMaster(client);

                    await modbus.WriteMultipleRegistersAsync(SlaveAddress, Offset, ushorts);
                    var result = await modbus.ReadHoldingRegistersAsync(SlaveAddress, Offset, 8);

                    bytes = FromShortArray(result);
                    var decoded = Encoding.ASCII.GetString(bytes);

                    if (decoded.StartsWith(parameter + "="))
                    {
                        var startIndex = parameter.Length + 1;
                        var indexOfNull =
                            decoded.IndexOf("\0", startIndex, StringComparison.InvariantCultureIgnoreCase);

                        var value = decoded.Substring(startIndex, indexOfNull - startIndex);
                        Log.Debug($"Value: {value}");
                        return value;
                    }
                }
            }

            return string.Empty;
        }

        private static ushort[] ToUShortArray(IEnumerable<byte> input)
        {
            var array = input.ToArray();
            if (array.Length % 2 == 1)
            {
                array = array.Concat(new byte[] {0x0}).ToArray();
            }

            var values = new List<ushort>();
            for (var i = 0; i < array.Length; i += 2)
            {
                var part = array.Skip(i).Take(2).ToArray();

                // not quite sure why I currently need to do this:
                part = part.Swap();

                values.Add(BitConverter.ToUInt16(part));
            }

            return values.ToArray();
        }

        private static byte[] FromShortArray(ushort[] shorts)
        {
            return shorts.SelectMany(s =>
            {
                // not quite sure why I currently need to swap:
                return BitConverter.GetBytes(s).Swap();
            }).ToArray();
        }
    }

    public static class ByteArrayExtensions
    {
        public static byte[] Swap(this byte[] array)
        {
            if (array.Length != 2)
            {
                throw new Exception("Only byte arrays with length 2 are possible to beeing swapped.");
            }

            return new[] {array[1], array[0]};
        }
    }
}