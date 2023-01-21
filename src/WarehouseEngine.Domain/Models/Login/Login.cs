using System.ComponentModel.DataAnnotations;

namespace WarehouseEngine.Domain.Models.Login;
public class Login
{
    [Required(ErrorMessage = "User Name is required")]
    public required string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public required string Password { get; set; }
}
