using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using TodoApi.Models.Entities;
using TodoApi.Models.DTOs;
using TodoApi.Data;

namespace TodoApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    
    // db context
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    // POST: users
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto)
    {
        // Check if format is corrrect.
        if(!ModelState.IsValid){
            return BadRequest(ModelState);
        }

        // Map DTO - Entity 
        var user = new User{
            Id = Guid.NewGuid().ToString(),
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = dto.Password,
            CreatedAt = DateTime.UtcNow
        };

        // GetUserDto to return to client.
        var userDto = new GetUserDto{
            Username = user.Username,
            CreatedAt = user.CreatedAt,
            Id = user.Id
        };

        // Add User to database.
        _context.Users.Add(user);
        
        // Write to SQLite file.
        await _context.SaveChangesAsync();

        return CreatedAtAction(
                nameof(GetUser),
                new { id = user.Id },
                userDto);

    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GetUserDto>>> GetUsers(){
        var users = await _context.Users.Select(u => new GetUserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    CreatedAt = u.CreatedAt
                }).ToListAsync();

        return Ok(users);
    }
    
    // GET : users/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<GetUserDto>> GetUser(string id){
        
        var user = await _context.Users.FindAsync(id);

        if (user == null){
            return NotFound();
        }

        var userDto = new GetUserDto{
            Id = user.Id,
            Username = user.Username,
            CreatedAt = user.CreatedAt
        };

        return Ok(userDto);
    }
    

    //
    [HttpPost("tasks/{id}")]
    public async Task<IActionResult> CreateTask(string id, [FromBody] CreateTaskDto dto){
    
        // Check if format is correct.
        if(!ModelState.IsValid){
            return BadRequest(ModelState);
        }

        var userTask = new UTask{
            Id = Guid.NewGuid().ToString(),
            UserId = id,
            Name = dto.Name,
            Description = dto.Description,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate
        };

        var taskDto = new GetTaskDto{
            Id = userTask.Id,
            Name = userTask.Name,
            Description = userTask.Description,
            StartDate = userTask.StartDate,
            EndDate = userTask.EndDate
        };

        _context.UTasks.Add(userTask);

        await _context.SaveChangesAsync();


        return CreatedAtAction(
                nameof(GetTasks),
                new { id = userTask.Id},
                taskDto);
    }

    // User access tasks
    [HttpGet("{id}/tasks")]
    public async Task<ActionResult<IEnumerable<GetTaskDto>>> GetTasks(string id)    {
   
    var tasks = await _context.UTasks.
        Where(u => u.UserId == id).
        Select(u => new GetTaskDto
                {
                    Id = u.Id,
                    Name = u.Name,
                    Description = u.Description,
                    StartDate = u.StartDate,
                    EndDate = u.EndDate
                }).ToListAsync();
    //Return tasks
    return Ok(tasks);
    }

}
