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

    public class Program
    {
        private const int TimeoutMs = 2000;

        private static IPAddress ipAddress;

        protected internal static TimeSpan QueryInterval;

        public static async Task Main(string[] args)
        {
            ConfigureServices();

            QueryInterval = TimeSpan.FromSeconds(double.Parse(Configuration["QueryInterval"]));
            ipAddress = IPAddress.Parse(Configuration["HeliosIP"]);

            var metricServer = new MetricServer("0.0.0.0", 9091);
            metricServer.Start();

            var cts = new CancellationTokenSource();

            await Task.WhenAll(
                //Task.Run(
                //    () =>
                //        {
                //            while (Console.ReadKey().Key != ConsoleKey.Escape)
                //            {
                //            }

                //            cts.Cancel();
                //        }),
                StartUpdateInterval(cts.Token));

            metricServer.Stop();
            Log.CloseAndFlush();
        }

        private static async Task StartUpdateInterval(CancellationToken cancellationToken)
        {
            var outside = Metrics.CreateGauge("helios_outside_air_temperature", "Aussenluft Temperatur");
            var incoming = Metrics.CreateGauge("helios_incoming_air_temperature", "Zuluft Temperatur");
            var exit = Metrics.CreateGauge("helios_exit_air_temperature", "Fortluft Temperatur");
            var outgoing = Metrics.CreateGauge("helios_outgoing_air_temperature", "Abluft Temperatur");

            var fanLevel = Metrics.CreateGauge("helios_fans_level", "Lüfterstufe");
            var fanPercentage = Metrics.CreateGauge("helios_fans_percentage", "Lüfterstufe");

            using (var client = new TcpClient())
            {
                client.ReceiveTimeout = TimeoutMs;
                client.SendTimeout = TimeoutMs;
                client.Client.Connect(ipAddress, HeliosDefaults.Port);

                while (!cancellationToken.IsCancellationRequested)
                {
                    Log.Information("Querying temperatures...");

                    var timestamp = await QueryHeliosValue(client, HeliosParameters.Uhrzeit);

                    var outsideValue = await QueryHeliosValue(client, HeliosParameters.AussenluftTemperatur);
                    outside.Set(outsideValue);

                    var incomingValue = await QueryHeliosValue(client, HeliosParameters.ZuluftTemperatur);
                    incoming.Set(incomingValue);

                    var exitValue = await QueryHeliosValue(client, HeliosParameters.FortluftTemperatur);
                    exit.Set(exitValue);

                    var outgoingValue = await QueryHeliosValue(client, HeliosParameters.AbluftTemperatur);
                    outgoing.Set(outgoingValue);

                    var fanLevelValue = await QueryHeliosValue(client, HeliosParameters.Lüfterstufe);
                    fanLevel.Set(fanLevelValue);

                    var fanPercentageValue = await QueryHeliosValue(client, HeliosParameters.ProzentualeLüfterstufe);
                    fanPercentage.Set(fanPercentageValue / (double) 100);

                    Log.Information($"{timestamp} - {outsideValue} / {incomingValue} / {exitValue} / {outgoingValue}");
                    try
                    {
                        await Task.Delay(QueryInterval, cancellationToken);
                    }
                    catch (TaskCanceledException)
                    {
                    }
                }
            }
        }

        private static void ConfigureServices()
        {
            // Read the application settings file.
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            // Setting up the static Serilog logger.
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration).CreateLogger();

            // Set the default culture.
            var ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }

        public static IConfigurationRoot Configuration { get; set; }

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

                Log.Debug($"Decoded: {decoded}");

                if (TryExtractValue(parameter.Code, decoded, out var value))
                {
                    Log.Debug($"Trying to convert {value} to {typeof(T)}");
                    return (T)Convert.ChangeType(value, typeof(T));
                }
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
                Log.Debug($"Answer does match requested parameter: {parameter}");
                var startIndex = parameter.Length + 1;
                var indexOfNull = decoded.IndexOfAny(new []{'\0','?'}, startIndex);
                Log.Debug($"Start: {startIndex}, End: {indexOfNull}");

                value = decoded.Substring(startIndex, indexOfNull - startIndex);
                Log.Debug($"Value: {value}");

                return true;
            }

            return false;
        }
    }
}