namespace TodoApi.Models.DTOs;

public class GetUserDto
{
    public string Id {get; set;} = null!;
    public string Username {get; set;} = null!;
    public DateTime CreatedAt {get; set;}
}
