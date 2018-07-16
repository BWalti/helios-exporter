namespace NModbusTCP.Pages
{
    #region Using Directives

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity.UI.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Configuration;

    using NModbusTCP.Models;

    #endregion

    /// <summary>
    /// The "code-behind" for the contact page.
    /// </summary>
    public class ContactModel : PageModel
    {
        #region Private Data Members

        private readonly ILogger _logger;
        private readonly IEmailSender _sender;
        private readonly AppSettings _settings = new AppSettings();

        #endregion

        #region Public Classes

        public class ContactFormModel
        {
            [Required(ErrorMessage = "a name is required.")]
            public string Name { get; set; }
            [Required(ErrorMessage = "a valid email is required.")]
            [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$")]
            public string Email { get; set; }
            [Required(ErrorMessage = "a message content is required.")]
            public string Message { get; set; }
        }

        #endregion

        #region Public Properties

        [BindProperty]
        public ContactFormModel Contact { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactModel"/> class.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        /// <param name="sender"></param>
        public ContactModel(IConfiguration configuration, ILogger<ContactModel> logger, IEmailSender sender)
        {
            configuration.GetSection("AppSettings")?.Bind(_settings);
            _logger = logger;
            _sender = sender;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// HTTP GET handler to initialize state needed for the page.
        /// </summary>
        public void OnGet()
        {

        }

        /// <summary>
        /// HTTP POST handler to process the request.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var address = _settings.Email.Address;
                var subject = _settings.Email.Subject;
                var message = $"Name: {Contact.Name}<br />Email: {Contact.Email}<br />Message:<br />{Contact.Message}";
                await _sender.SendEmailAsync(address, subject, message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Sending email not successful.");
            }

            return RedirectToPage("Index");
        }

        #endregion
    }
}