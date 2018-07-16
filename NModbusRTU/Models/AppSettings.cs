// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace NModbusRTU.Models
{
    /// <summary>
    /// Helper class to hold application specific settings.
    /// </summary>
    public class AppSettings
    {
        #region Public Properties

        /// <summary>
        /// The Contact email options.
        /// </summary>
        public EmailOptions Email { get; set; } = new EmailOptions();

        /// <summary>
        /// The MODBUS RTU master configuration.
        /// </summary>
        public ModbusMaster Master { get; set; } = new ModbusMaster();

        /// <summary>
        /// The MODBUS RTU slave configuration.
        /// </summary>
        public ModbusSlave Slave { get; set; } = new ModbusSlave();

        /// <summary>
        /// The email sender authentication data.
        /// </summary>
        public AuthenticationData Authentication { get; set; } = new AuthenticationData();

        /// <summary>
        /// The Swagger options.
        /// </summary>
        public SwaggerOptions Swagger { get; set; } = new SwaggerOptions();

        #endregion
    }
}
