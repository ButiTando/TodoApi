using System.ComponentModel.DataAnnotations;
using System;

namespace TodoApi.Models.DTOs;

public class CreateTaskDto
{
    [Required]
    [StringLength(100)]
    public string Name {get; set;} = null!;

    [StringLength(500)]
    public string? Description {get; set;}

    public DateTime? StartDate {get; set;}

    public DateTime? EndDate {get; set;}
}
