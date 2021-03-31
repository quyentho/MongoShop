using System.ComponentModel.DataAnnotations;

namespace MongoShop.Models.Account
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
