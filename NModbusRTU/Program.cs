// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace NModbusRTU
{
    #region Using Directives

    using System;
    using System.Globalization;
    using System.IO;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;

    using Serilog;
    using NModbusRTU.Models;

    #endregion

    /// <summary>
    /// The main ASP.NET application.
    /// </summary>
    public static class Program
    {
        #region Private Data Members

        private static readonly AppSettings _settings = new AppSettings();

        #endregion

        #region Public Methods

        /// <summary>
        /// The main entry point.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            // Read the application settings file.
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            configuration.GetSection("AppSettings")?.Bind(_settings);

            // Setting up the static Serilog logger.
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            // Set the default culture.
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            try
            {
                var host = CreateWebHostBuilder(args).Build();
                Log.Information("Starting Web host.");
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Exception in Web host.");
            }
            finally
            {
                Log.Information("Web host terminated.");
                Log.CloseAndFlush();
            }
        }

        /// <summary>
        /// The default web host builder.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog();

        #endregion
    }
}
