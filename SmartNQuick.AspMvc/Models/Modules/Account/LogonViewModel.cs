//@BaseCode
//MdStart
#if ACCOUNT_ON
using System.ComponentModel.DataAnnotations;

namespace SmartNQuick.AspMvc.Models.Modules.Account
{
    public partial class LogonViewModel : ModelObject
    {
        public string ReturnUrl { get; set; }
        public string IdentityUrl { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
#endif
//MdEnd
