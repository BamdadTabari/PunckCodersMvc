using System.ComponentModel.DataAnnotations;

namespace DataProvider.Models.Command.Identity;
public class PasswordResetCommand
{
    [Required]
    public string EmailOrUserName { get; set; }
    [Required]
    public string NewPassword { get; set; }
    public string? ConfimCode { get; set; }

}
