namespace Demo.Domain.Dtos;

/// <summary>
/// Response model for user operations
/// </summary>
public class UserResponse
{
    /// <summary>
    /// Unique identifier for the user
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// User's full name
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// User's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// User's phone number
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// User's date of birth
    /// </summary>
    public DateTime? DateOfBirth { get; set; }

    /// <summary>
    /// Whether the user is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// When the user was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// When the user was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
