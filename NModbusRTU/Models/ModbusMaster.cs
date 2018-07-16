// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModbusMaster.cs" company="DTV-Online">
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
    /// Helper class holding Modbus RTU master data.
    /// </summary>
    public class ModbusMaster
    {
        #region Public Properties

        public string SerialPort { get; set; } = string.Empty;
        public int Baudrate { get; set; } = 9600;
        public Parity Parity { get; set; } = Parity.None;
        public int DataBits { get; set; } = 8;
        public StopBits StopBits { get; set; } = StopBits.One;
        public int ReadTimeout { get; set; } = -1;
        public int WriteTimeout { get; set; } = -1;

        #endregion
    }
}
