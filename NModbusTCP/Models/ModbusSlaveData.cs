// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModbusSlaveData.cs" company="DTV-Online">
//   Copyright(c) 2017 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace NModbusTCP.Models
{
    /// <summary>
    /// Helper class to hold all Modbus slave specific information.
    /// </summary>
    public class ModbusSlaveData
    {
        #region Public Properties

        /// <summary>
        /// The Modbus slave IP address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// The Modbus slave IP Port number (typically 502).
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// The Modbus slave ID.
        /// </summary>
        public byte ID { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ModbusSlaveData"/> class.
        /// </summary>
        public ModbusSlaveData() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModbusSlaveData"/> class.
        /// Note that the application settings provides default values.
        /// </summary>
        /// <param name="settings">The application settings.</param>
        /// <param name="address">The Modbus slave IP address.</param>
        /// <param name="port">The Modbus slave port number.</param>
        /// <param name="id">The Modbus slave ID.</param>
        public ModbusSlaveData(AppSettings settings, string address, int? port, byte? id)
        {
            this.Address = string.IsNullOrEmpty(address) ? settings.Slave.Address : address;
            this.Port = port ?? settings.Slave.Port;
            this.ID = id ?? settings.Slave.SlaveID;
        }

        #endregion
    }
}
