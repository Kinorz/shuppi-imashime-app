using System.ComponentModel.DataAnnotations;

namespace ShuppiApi.Dtos;

public class UserRegisterDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = null!;
}
