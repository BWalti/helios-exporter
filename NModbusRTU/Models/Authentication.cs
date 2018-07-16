// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Authentication.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace NModbusRTU.Models
{
    /// <summary>
    /// The email authetication data.
    /// </summary>
    public class AuthenticationData
    {
        #region Public Properties

        public string ClientID { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;

        #endregion
    }
}
