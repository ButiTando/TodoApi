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

    // Create a user.
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
    
    // Get all users.
    // GET: /users
    // TODO: Limit access to admin users.
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
    // Get specific user. 
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


    // Delete specific user
    // DELETE: users/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(string id){

        var user = await _context.Users.FindAsync(id);

        if(user == null){
            return NotFound("User not found.");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    
        return NoContent();
    }
    
    // Create a new task for specific user.
    //POST: /{user_id}/tasks/
    [HttpPost("{id}/tasks")]
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
    
    // Get all tasks for a specific user.
    // GET: users/{user_id}/tasks
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
    
    return Ok(tasks);
    }
    
    // Delete task from specific user.
    // DELETE: users/{user_id}/tasks/{task_id}
    [HttpDelete("{user_id}/tasks/{task_id}")]
    public async Task<IActionResult> RemoveTask(string user_id, string task_id){
    
    var utask = await _context.UTasks.FirstOrDefaultAsync(u => u.UserId == user_id && u.Id == task_id);

    if(utask == null){
        return NotFound("Task not found");
    }

    _context.UTasks.Remove(utask);
    await _context.SaveChangesAsync();

    return NoContent();    
    }

}
