using System.ComponentModel.DataAnnotations;

namespace DataProvider.Models.Command.Identity
{
    public class LoginCommand
    {
        [Required]
        public string EmailOrUserName { get; set; }

        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
