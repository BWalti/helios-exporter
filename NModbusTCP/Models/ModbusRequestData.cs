// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModbusRequestData.cs" company="DTV-Online">
//   Copyright(c) 2017 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace NModbusTCP.Models
{
    /// <summary>
    /// Helper class to hold all Modbus request data.
    /// </summary>
    public class ModbusRequestData
    {
        #region Public Properties

        /// <summary>
        /// The Modbus slave id.
        /// </summary>
        public ModbusSlaveData Slave { get; set; } = new ModbusSlaveData();

        /// <summary>
        /// The Modbus address of the first data item (offset).
        /// </summary>
        public ushort Offset { get; set; }

        /// <summary>
        /// The number of Modbus data items requested.
        /// </summary>
        public ushort Number { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ModbusRequestData"/> class.
        /// </summary>
        public ModbusRequestData() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModbusRequestData"/> class.
        /// </summary>
        /// <param name="slave">The Modbus slave data.</param>
        /// <param name="offset">The Modbus address of the first item requested.</param>
        /// <param name="number">The number of Modbus data items requested.</param>
        public ModbusRequestData(ModbusSlaveData slave, int offset, int number = 1)
        {
            this.Slave = slave;
            this.Offset = (ushort)offset;
            this.Number = (ushort)number;
        }

        #endregion
    }
}
