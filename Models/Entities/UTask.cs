using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models.Entities;

public class UTask{
    
    // Required fields.
    [Key]
    [Required]
    public string Id {get; init;} = null!;
    
    [Required]
    public string UserId {get; init;} = null!;

    [Required]
    public string Name {get; set;} = null!;
    
    // Not required fields.
    public string? Description {get; set;}
    
    // Date time fields.
    public DateTime? StartDate {get; set;}
    public DateTime? EndDate {get; set;}

}
