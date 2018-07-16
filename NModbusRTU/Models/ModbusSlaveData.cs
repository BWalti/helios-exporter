// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModbusSlaveData.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace NModbusRTU.Models
{
    /// <summary>
    /// Helper class to hold all Modbus slave specific information.
    /// </summary>
    public class ModbusSlaveData
    {
        /// <summary>
        /// The Modbus slave ID.
        /// </summary>
        public byte ID { get; set; } = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModbusSlaveData"/> class.
        /// </summary>
        public ModbusSlaveData() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModbusSlaveData"/> class.
        /// Note that the application settings provides default values.
        /// </summary>
        /// <param name="settings">The application settings.</param>
        /// <param name="id">The Modbus slave ID.</param>
        public ModbusSlaveData(AppSettings settings, byte? id)
        {
            this.ID = id ?? settings.Slave.SlaveID;
        }
    }
}
