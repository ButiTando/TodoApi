using System;

namespace TodoApi.Models.DTOs;

public class GetTaskDto
{
    public string Id {get; set;} = null!;
    public string Name {get; set;} = null!;
    public string? Description {get; set;} 
    public DateTime? StartDate {get; set;}
    public DateTime? EndDate {get; set;}
}
