using Microsoft.AspNetCore.Mvc;
using Demo.Web.Api.Models;
using System.ComponentModel.DataAnnotations;
using Demo.Application.Services;
using Demo.Domain.Dtos;

namespace Demo.Web.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(
    IUserService userService,
    ILogger<UserController> logger) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly ILogger<UserController> _logger = logger;

    [HttpGet]
    public async Task<IActionResult> GetAllUsers([FromQuery] bool includeInactive = false)
    {
        try
        {
            var users = await _userService.GetAllUsersAsync(includeInactive);
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users");
            return StatusCode(500, "An error occurred while retrieving users");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound($"User with ID '{id}' not found");
            }

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with ID: {UserId}", id);
            return StatusCode(500, "An error occurred while retrieving the user");
        }
    }

    [HttpGet("by-email/{email}")]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        try
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"User with email '{email}' not found");
            }

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with email: {Email}", email);
            return StatusCode(500, "An error occurred while retrieving the user");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.CreateUserAsync(request);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }
        catch (ValidationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user");
            return StatusCode(500, "An error occurred while creating the user");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userService.UpdateUserAsync(id, request);
            if (user == null)
            {
                return NotFound($"User with ID '{id}' not found");
            }

            return Ok(user);
        }
        catch (ValidationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with ID: {UserId}", id);
            return StatusCode(500, "An error occurred while updating the user");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        try
        {
            var deleted = await _userService.DeleteUserAsync(id);
            if (!deleted)
            {
                return NotFound($"User with ID '{id}' not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user with ID: {UserId}", id);
            return StatusCode(500, "An error occurred while deleting the user");
        }
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchUsers([FromQuery] [Required] string searchTerm)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return BadRequest("Search term cannot be empty");
            }

            if (searchTerm.Length < 2)
            {
                return BadRequest("Search term must be at least 2 characters long");
            }

            var users = await _userService.SearchUsersAsync(searchTerm);
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching users with term: {SearchTerm}", searchTerm);
            return StatusCode(500, "An error occurred while searching users");
        }
    }
}
