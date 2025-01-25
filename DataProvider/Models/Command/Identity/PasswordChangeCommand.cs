using System.ComponentModel.DataAnnotations;

namespace DataProvider.Models.Command.Identity;
public class PasswordChangeCommand
{
    [Required]
    public int UserId { get; set; }
    [Required]
    public string OldPassword { get; set; }
    [Required]
    public string NewPassword { get; set; }
}
