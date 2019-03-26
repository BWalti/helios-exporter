namespace Helios_PoC
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
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

        private static TimeSpan QueryInterval => TimeSpan.FromSeconds(double.Parse(Configuration["QueryInterval"]));

        private static IPAddress HeliosIpAddress => IPAddress.Parse(Configuration["HeliosIP"]);

        public static IConfigurationRoot Configuration { get; set; }

        public static async Task Main(string[] args)
        {
            ConfigureServices();

            ipAddress = HeliosIpAddress;

            var metricServer = new MetricServer("0.0.0.0", 9091);
            metricServer.Start();

            var cts = new CancellationTokenSource();

            await StartUpdateInterval(cts.Token);

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

            var bypassMinTemp =
                Metrics.CreateGauge("helios_bypass_min_outside_temperature", "Bypass: Minimale Aussentemperatur");
            var bypassRoomTemp = Metrics.CreateGauge("helios_bypass_room_temperature", "Bypass: Raumtemperatur");

            using (var client = new TcpClient())
            {
                client.ReceiveTimeout = TimeoutMs;
                client.SendTimeout = TimeoutMs;
                client.Client.Connect(ipAddress, HeliosDefaults.Port);

                var modbus = new ModbusFactory().CreateMaster(client);

                while (!cancellationToken.IsCancellationRequested)
                {
                    Log.Information("Querying temperatures...");

                    var timestamp = await modbus.QueryHeliosValue(HeliosParameters.Uhrzeit);

                    var outsideValue = await modbus.QueryHeliosValue(HeliosParameters.AussenluftTemperatur);
                    outside.Set(outsideValue);

                    var incomingValue = await modbus.QueryHeliosValue(HeliosParameters.ZuluftTemperatur);
                    incoming.Set(incomingValue);

                    var exitValue = await modbus.QueryHeliosValue(HeliosParameters.FortluftTemperatur);
                    exit.Set(exitValue);

                    var outgoingValue = await modbus.QueryHeliosValue(HeliosParameters.AbluftTemperatur);
                    outgoing.Set(outgoingValue);

                    var fanLevelValue = await modbus.QueryHeliosValue(HeliosParameters.Lüfterstufe);
                    fanLevel.Set(fanLevelValue);

                    var bypassMin = await modbus.QueryHeliosValue(HeliosParameters.BypassMinAussentemperaturTemperatur);
                    bypassMinTemp.Set(bypassMin);

                    var bypassRaum = await modbus.QueryHeliosValue(HeliosParameters.BypassRaumTemperatur);
                    bypassRoomTemp.Set(bypassRaum);

                    var fanPercentageValue = await modbus.QueryHeliosValue(HeliosParameters.ProzentualeLüfterstufe);
                    fanPercentage.Set(fanPercentageValue / (double)100);

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
            Configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                      .AddJsonFile("appsettings.json", true, true)
                                                      .AddEnvironmentVariables()
                                                      .Build();

            // Setting up the static Serilog logger.
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration)
                                                  .CreateLogger();

            // Set the default culture.
            var ci = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }
    }
}