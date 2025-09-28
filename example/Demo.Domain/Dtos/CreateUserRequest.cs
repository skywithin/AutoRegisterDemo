using System.ComponentModel.DataAnnotations;

namespace Demo.Domain.Dtos;

/// <summary>
/// Request model for creating a new user
/// </summary>
public class CreateUserRequest
{
    /// <summary>
    /// User's first name
    /// </summary>
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// User's last name
    /// </summary>
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// User's email address
    /// </summary>
    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// User's phone number
    /// </summary>
    [Phone]
    [StringLength(20)]
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// User's date of birth
    /// </summary>
    public DateTime? DateOfBirth { get; set; }
}
