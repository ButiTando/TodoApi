using System.ComponentModel.DataAnnotations;

public class CreateUserDto
{
    [Required]
    [MaxLength(50)]
    public string Username{ get; set;} = null!;

    [Required]
    [EmailAddress]
    public string Email{ get; set;} = null!;
    
    [Required]
    [MinLength(6)]
    public string Password{ get; set;} = null!;
}
