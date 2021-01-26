using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PressCenter.Web.ViewModels.Contact
{
    public class ContactFormViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your names")]
        [Display(Name = "Your names")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Please enter valid email address")]
        [Display(Name = "Your email address")]
        public string Email { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Message title")]
        [StringLength(100, ErrorMessage = "The title must be at least {2} and no more than {1} characters.", MinimumLength = 3)]
        [Display(Name = "Message title")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the content of the message")]
        [StringLength(10000, ErrorMessage = "The message must be at least {2} characters.", MinimumLength = 20)]
        [Display(Name = "Message")]
        public string Content { get; set; }

        public string Ip { get; set; }

        //[GoogleReCaptchaValidation]
        public string RecaptchaValue { get; set; }
    }
}
