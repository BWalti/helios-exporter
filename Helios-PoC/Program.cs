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
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;

    using NModbus;

    using Prometheus.Client;
    using Prometheus.Client.MetricServer;

    using Serilog;

    public static class HeliosDefaults
    {
        public const int Offset = 1;

        public const int Port = 502;

        public const byte SlaveAddress = 180;
    }

    public class Program
    {
        private const int TimeoutMs = 2000;

        private static readonly IPAddress IpAddress = IPAddress.Parse("192.168.0.228");

        public static async Task Main(string[] args)
        {
            ConfigureServices();

            var metricServer = new MetricServer("0.0.0.0", 49091);
            metricServer.Start();

            var temperaturAussenluft = Metrics.CreateGauge("temp:from:outside", "Aussenluft Temperatur");
            var temperaturZuluft = Metrics.CreateGauge("temp:to:inside", "Zuluft Temperatur");
            var temperaturFortluft = Metrics.CreateGauge("temp:to:outside", "Fortluft Temperatur");
            var temperaturAbluft = Metrics.CreateGauge("temp:from:inside", "Abluft Temperatur");

            var lüfterStufe = Metrics.CreateGauge("fan:level", "Lüfterstufe");
            var prozentualeLüfterStufe = Metrics.CreateGauge("fan:percentage", "Lüfterstufe");

            var cts = new CancellationTokenSource();

            await Task.WhenAll(
                Task.Run(
                    () =>
                        {
                            while(Console.ReadKey().Key != ConsoleKey.Escape)
                            {
                            }

                            cts.Cancel();
                        }),
                Task.Run(
                    async () =>
                        {
                            using (var client = new TcpClient())
                            {
                                client.ReceiveTimeout = TimeoutMs;
                                client.SendTimeout = TimeoutMs;
                                client.Client.Connect(IpAddress, HeliosDefaults.Port);

                                while (!cts.IsCancellationRequested)
                                {
                                    Log.Information("Querying temperatures...");

                                    var uhrzeit = await QueryHeliosValue(client, HeliosVariables.Uhrzeit);

                                    var aussenluft = await QueryHeliosValue(
                                                         client,
                                                         HeliosVariables.AussenluftTemperatur);
                                    temperaturAussenluft.Set(aussenluft);

                                    var zuluft = await QueryHeliosValue(client, HeliosVariables.ZuluftTemperatur);
                                    temperaturZuluft.Set(zuluft);

                                    var fortluft = await QueryHeliosValue(client, HeliosVariables.FortluftTemperatur);
                                    temperaturFortluft.Set(fortluft);

                                    var abluft = await QueryHeliosValue(client, HeliosVariables.AbluftTemperatur);
                                    temperaturAbluft.Set(abluft);

                                    var a = await QueryHeliosValue(client, HeliosVariables.Lüfterstufe);
                                    lüfterStufe.Set(a);

                                    var b = await QueryHeliosValue(client, HeliosVariables.ProzentualeLüfterstufe);
                                    prozentualeLüfterStufe.Set(b / (double)100);

                                    Log.Information($"{uhrzeit} - {aussenluft} / {zuluft} / {fortluft} / {abluft}");
                                    try
                                    {
                                        await Task.Delay(TimeSpan.FromSeconds(30), cts.Token);
                                    }
                                    catch (TaskCanceledException)
                                    {
                                    }
                                }
                            }
                        }));

            metricServer.Stop();
            Log.CloseAndFlush();
        }

        private static void ConfigureServices()
        {
            // Read the application settings file.
            var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true).AddEnvironmentVariables().Build();

            // Setting up the static Serilog logger.
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();

            // Set the default culture.
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
        }

        private static byte[] FromShortArray(ushort[] shorts)
        {
            return shorts.SelectMany(
                s =>
                    {
                        // not quite sure why I currently need to swap:
                        return BitConverter.GetBytes(s).Swap();
                    }).ToArray();
        }

        private static async Task<T> QueryHeliosValue<T>(TcpClient client, VariableDeclaration<T> parameter)
        {
            Log.Debug($"Querying {parameter.Code}...");
            var bytes = Encoding.ASCII.GetBytes($"{parameter.Code}\0");
            var ushorts = ToUShortArray(bytes);

            if (client.Connected)
            {
                var factory = new ModbusFactory();
                var modbus = factory.CreateMaster(client);

                await modbus.WriteMultipleRegistersAsync(HeliosDefaults.SlaveAddress, HeliosDefaults.Offset, ushorts);
                var result = await modbus.ReadHoldingRegistersAsync(
                                 HeliosDefaults.SlaveAddress,
                                 HeliosDefaults.Offset,
                                 parameter.RegisterCount);

                bytes = FromShortArray(result);
                var decoded = Encoding.ASCII.GetString(bytes);

                if (TryExtractValue(parameter.Code, decoded, out var value))
                    return (T)Convert.ChangeType(value, typeof(T));
            }

            return default;
        }

        private static ushort[] ToUShortArray(IEnumerable<byte> input)
        {
            var array = input.ToArray();
            if (array.Length % 2 == 1) array = array.Concat(new byte[] { 0x0 }).ToArray();

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

        private static bool TryExtractValue(string parameter, string decoded, out string value)
        {
            value = string.Empty;

            if (decoded.StartsWith(parameter + "="))
            {
                var startIndex = parameter.Length + 1;
                var indexOfNull = decoded.IndexOf("\0", startIndex, StringComparison.InvariantCulture);

                value = decoded.Substring(startIndex, indexOfNull - startIndex);
                Log.Debug($"Value: {value}");
                {
                    return true;
                }
            }

            return false;
        }
    }
}