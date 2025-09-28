using System.ComponentModel.DataAnnotations;

namespace Demo.Domain.Models;

/// <summary>
/// Represents a user in the system
/// </summary>
public class User
{
    /// <summary>
    /// Unique identifier for the user
    /// </summary>
    public Guid Id { get; set; }

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

    /// <summary>
    /// Whether the user is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// When the user was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// When the user was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// User's full name
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";
}
