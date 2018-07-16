// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmailOptions.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace NModbusRTU.Models
{
    /// <summary>
    /// Helper class holding contact request email settings.
    /// </summary>
    public class EmailOptions
    {
        #region Public Properties

        public int SmtpPort { get; set; } = 587;
        public string SmtpServer { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;

        #endregion
    }
}
