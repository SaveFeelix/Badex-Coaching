using System.ComponentModel.DataAnnotations;

namespace Models.Dto.Users;

public class UserGetDto
{
    public UserGetDto(int id, string firstName, string lastName, string? email, string? phone, string userName, bool isAdmin)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        UserName = userName;
        IsAdmin = isAdmin;
    }

    public UserGetDto()
    {
    }

    [Required]
    public int Id { get; set; }
    
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