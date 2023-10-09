using System.ComponentModel.DataAnnotations;

namespace Models.Dto.Users;

public class UserLoginDto
{
    [Required] public string Username { get; set; }

    [Required] [MinLength(8)] public string Password { get; set; }
}