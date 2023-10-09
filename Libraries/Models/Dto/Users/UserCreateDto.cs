using System.ComponentModel.DataAnnotations;

namespace Models.Dto.Users;

public class UserCreateDto
{
    public UserCreateDto(string firstName, string lastName, string? email, string? phone, string userName, bool isAdmin)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        UserName = userName;
        IsAdmin = isAdmin;
    }

    public UserCreateDto()
    {
    }

    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    
    [Required]
    public string UserName { get; set; }
    
    [Required]
    public bool IsAdmin { get; set; }
}