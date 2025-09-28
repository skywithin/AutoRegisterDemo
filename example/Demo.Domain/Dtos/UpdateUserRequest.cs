using System.ComponentModel.DataAnnotations;

namespace Demo.Domain.Dtos;

/// <summary>
/// Request model for updating a user
/// </summary>
public class UpdateUserRequest
{
    /// <summary>
    /// User's first name
    /// </summary>
    [StringLength(50, MinimumLength = 2)]
    public string? FirstName { get; set; }

    /// <summary>
    /// User's last name
    /// </summary>
    [StringLength(50, MinimumLength = 2)]
    public string? LastName { get; set; }

    /// <summary>
    /// User's email address
    /// </summary>
    [EmailAddress]
    [StringLength(100)]
    public string? Email { get; set; }

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
    public bool? IsActive { get; set; }
}
