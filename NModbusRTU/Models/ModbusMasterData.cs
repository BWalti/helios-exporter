// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModbusMasterData.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace NModbusRTU.Models
{
    using System.IO.Ports;

    /// <summary>
    /// Helper class to hold all Modbus RTU master specific information.
    /// </summary>
    public class ModbusMasterData
    {
        /// <summary>
        /// The Modbus RTU master COM port.
        /// </summary>
        public string SerialPort { get; set; } = string.Empty;

        /// <summary>
        /// The Modbus RTU baud number (typically 9600).
        /// </summary>
        public int Baudrate { get; set; } = 9600;

        /// <summary>
        /// The Modbus RTU parity.
        /// </summary>
        public Parity Parity { get; set; } = Parity.None;

        /// <summary>
        /// The number of data bits.
        /// </summary>
        public int DataBits { get; set; } = 8;

        /// <summary>
        /// The number of stop bits.
        /// </summary>
        public StopBits StopBits { get; set; } = StopBits.One;

        /// <summary>
        /// The read timeout (msec).
        /// </summary>
        public int ReadTimeout { get; set; } = -1;

        /// <summary>
        /// The write timeout (msec).
        /// </summary>
        public int WriteTimeout { get; set; } = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModbusMasterData"/> class.
        /// </summary>
        public ModbusMasterData() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModbusMasterData"/> class.
        /// Note that the application settings provides default values.
        /// </summary>
        /// <param name="settings">The application settings.</param>
        public ModbusMasterData(AppSettings settings)
        {
            this.SerialPort = settings.Master.SerialPort;
            this.Baudrate = settings.Master.Baudrate;
            this.Parity = settings.Master.Parity;
            this.DataBits = settings.Master.DataBits;
            this.StopBits = settings.Master.StopBits;
            this.ReadTimeout = settings.Master.ReadTimeout;
            this.WriteTimeout = settings.Master.WriteTimeout;
        }
    }
}
