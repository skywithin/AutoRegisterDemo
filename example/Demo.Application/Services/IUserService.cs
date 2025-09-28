using Demo.Domain.Dtos;

namespace Demo.Application.Services;

/// <summary>
/// Service for managing users
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Gets all users
    /// </summary>
    /// <param name="includeInactive">Whether to include inactive users</param>
    /// <returns>List of users</returns>
    Task<IEnumerable<UserResponse>> GetAllUsersAsync(bool includeInactive = false);

    /// <summary>
    /// Gets a user by ID
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>User if found, null otherwise</returns>
    Task<UserResponse?> GetUserByIdAsync(Guid id);

    /// <summary>
    /// Gets a user by email
    /// </summary>
    /// <param name="email">User email</param>
    /// <returns>User if found, null otherwise</returns>
    Task<UserResponse?> GetUserByEmailAsync(string email);

    /// <summary>
    /// Creates a new user
    /// </summary>
    /// <param name="request">User creation request</param>
    /// <returns>Created user</returns>
    Task<UserResponse> CreateUserAsync(CreateUserRequest request);

    /// <summary>
    /// Updates an existing user
    /// </summary>
    /// <param name="id">User ID</param>
    /// <param name="request">User update request</param>
    /// <returns>Updated user if found, null otherwise</returns>
    Task<UserResponse?> UpdateUserAsync(Guid id, UpdateUserRequest request);

    /// <summary>
    /// Deletes a user
    /// </summary>
    /// <param name="id">User ID</param>
    /// <returns>True if deleted, false if not found</returns>
    Task<bool> DeleteUserAsync(Guid id);

    /// <summary>
    /// Searches users by name or email
    /// </summary>
    /// <param name="searchTerm">Search term</param>
    /// <returns>Matching users</returns>
    Task<IEnumerable<UserResponse>> SearchUsersAsync(string searchTerm);
}
