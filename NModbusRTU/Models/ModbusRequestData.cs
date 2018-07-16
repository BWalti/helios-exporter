// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModbusRequestData.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace NModbusRTU.Models
{
    /// <summary>
    /// Helper class to hold all Modbus request data.
    /// </summary>
    public class ModbusRequestData
    {
        /// <summary>
        /// The Modbus slave id.
        /// </summary>
        public ModbusMasterData Master { get; set; } = new ModbusMasterData();

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

        /// <summary>
        /// Initializes a new instance of the <see cref="ModbusRequestData"/> class.
        /// </summary>
        public ModbusRequestData() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModbusRequestData"/> class.
        /// </summary>
        /// <param name="master">The Modbus master data.</param>
        /// <param name="slave">The Modbus slave data.</param>
        /// <param name="offset">The Modbus address of the first item requested.</param>
        /// <param name="number">The number of Modbus data items requested.</param>
        public ModbusRequestData(ModbusMasterData master, ModbusSlaveData slave, int offset, int number)
        {
            this.Master = master;
            this.Slave = slave;
            this.Offset = (ushort)offset;
            this.Number = (ushort)number;
        }
    }
}
