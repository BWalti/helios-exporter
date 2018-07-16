// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebApi.cshtml.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace NModbusRTU.Pages
{
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Authorization;
    using NModbusRTU.Extensions;

    [Authorize]
    public class WebApiModel : PageModel
    {
        public void OnGet()
        {
            SwaggerAuthorizeExtensions.IsUserAuthenticated = User.Identity.IsAuthenticated;
        }
    }
}