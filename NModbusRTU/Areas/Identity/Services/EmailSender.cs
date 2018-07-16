// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EmailSender.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace NModbusRTU.Areas.Identity.Services
{
    #region Using Directives

    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.Extensions.Configuration;

    using MailKit.Net.Smtp;
    using MimeKit;

    using NModbusRTU.Models;

    #endregion

    /// <summary>
    /// This class is used by the application to send email for account confirmation and password reset.
    /// For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    /// </summary>
    public class EmailSender : IEmailSender
    {
        #region Private Data Members

        private AppSettings _settings = new AppSettings();

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public EmailSender(IConfiguration configuration)
        {
            configuration.GetSection("AppSettings")?.Bind(_settings);
            Authentication = _settings.Authentication;
            Email = _settings.Email;
        }

        #endregion

        #region Public Properties

        public AuthenticationData Authentication { get; }
        public EmailOptions Email { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Implementation of sending an email using MailKit.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var mimemessage = new MimeMessage();

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = message
            };

            mimemessage.From.Add(new MailboxAddress(Authentication.ClientID));
            mimemessage.To.Add(new MailboxAddress(email));
            mimemessage.Subject = subject;
            mimemessage.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(Email.SmtpServer, Email.SmtpPort, useSsl: false).ConfigureAwait(false);
                await client.AuthenticateAsync(Authentication.ClientID, Authentication.ClientSecret);
                await client.SendAsync(mimemessage).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }

        #endregion
    }
}
