using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using PasswordGenerator;
using Server.Db.Models.Base;
using Server.Settings;

namespace Server.Db.Models;

public class UserModel : BaseModel
{
    public UserModel(string firstName, string lastName, string? email, string? phone, string userName, bool isAdmin, bool undeletable)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        UserName = userName;
        IsAdmin = isAdmin;
        Undeletable = undeletable;
    }

    public UserModel()
    {
    }

    [Required]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    public string LastName { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string? Phone { get; set; } = string.Empty;

    
    [Required]
    public string UserName { get; set; } = string.Empty;
    
    [Required]
    public byte[] Hash { get; set; } = Array.Empty<byte>();
    
    [Required]
    public byte[] Salt { get; set; } = Array.Empty<byte>();
    
    [Required]
    public bool IsAdmin { get; set; }


    public async Task<byte[]> GenerateHash(string password, bool useExistingKey = false)
    {
        using HMACSHA512 hmac = new HMACSHA512();
        if (useExistingKey)
            hmac.Key = Salt;
        else
            Salt = hmac.Key;

        byte[] bytes = Encoding.UTF8.GetBytes(password);
        await using var stream = new MemoryStream(bytes);
        byte[] hash = await hmac.ComputeHashAsync(stream);
        return hash;
    }

    public async Task<string> GeneratePassword(int length = 16)
    {
        Password generator = new Password(true, true, true, false, length);
        string? password = generator.Next();
        while (string.IsNullOrEmpty(password))
            password = generator.Next();
        Hash = await GenerateHash(password);
        return password;
    }

    public string GenerateToken(JwtSettings settings)
    {
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.UTF8.GetBytes(settings.Secret);
        SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(nameof(Id), Id.ToString()),
                new(nameof(IsAdmin), IsAdmin.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(settings.Duration),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
        };
        JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
        return handler.WriteToken(token);
    }
}