using Helios.Api;
using Helios.Modbus;
using Helios.Modbus.Internals;
using NModbus;
using Prometheus;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

Log.Information("Starting up");

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console()
    .ReadFrom.Configuration(ctx.Configuration));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddHealthChecks()
    .AddCheck<HeliosMetricsExporterHealthCheck>("Helios");

builder.Services.AddOptions<HeliosClientOptions>().BindConfiguration(nameof(HeliosClientOptions));
builder.Services.AddOptions<HeliosMetricsExporterOptions>().BindConfiguration(nameof(HeliosMetricsExporterOptions));

builder.Services.AddHostedService<HeliosMetricsExporter>();

builder.Services.AddSingleton<HeliosMetricsExporterState>();
builder.Services.AddSingleton<IModbusLogger, SerilogAdapter>();
builder.Services.AddSingleton<HeliosClient>();

try
{
    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSerilogRequestLogging();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapHealthChecks("/healthz");
    app.MapGet("/", () => "Hello World!");
    app.MapMetrics();
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}