// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModbusSlave.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace NModbusRTU.Models
{
    /// <summary>
    /// Helper class holding Modbus RTU slave data.
    /// </summary>
    public class ModbusSlave
    {
        #region Public Properties

        public byte SlaveID { get; set; } = 1;

        #endregion
    }
}
