using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models.Entities;

public class User{
    [Key]
    [Required]
    public string Id {get; init;} = null!;
    [Required]
    public string Username {get; set;} = null!;
    [Required]
    public string Email {get; set;} = null!;
    [Required]
    public string PasswordHash {get; set;} = null!;
    [Required]
    public DateTime CreatedAt {get; set;}
}
