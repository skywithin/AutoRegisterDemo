using AutoRegister.DI;
using Demo.Web.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace Demo.Web.Api.Services;

/// <summary>
/// Implementation of user service with in-memory storage
/// </summary>
[AutoRegister(Lifetime.Scoped, RegisterAs.Interface)]
public class UserService : IUserService
{
    private readonly List<User> _users = new();
    private readonly ILogger<UserService> _logger;

    /// <summary>
    /// Initializes a new instance of the UserService
    /// </summary>
    /// <param name="logger">Logger instance</param>
    public UserService(ILogger<UserService> logger)
    {
        _logger = logger;
        SeedData();
    }

    /// <summary>
    /// Gets all users
    /// </summary>
    public async Task<IEnumerable<UserResponse>> GetAllUsersAsync(bool includeInactive = false)
    {
        _logger.LogInformation("Getting all users, includeInactive: {IncludeInactive}", includeInactive);
        
        await Task.Delay(50); // Simulate async operation
        
        var users = _users.AsQueryable();
        
        if (!includeInactive)
        {
            users = users.Where(u => u.IsActive);
        }

        return users.Select(MapToResponse);
    }

    /// <summary>
    /// Gets a user by ID
    /// </summary>
    public async Task<UserResponse?> GetUserByIdAsync(Guid id)
    {
        _logger.LogInformation("Getting user by ID: {UserId}", id);
        
        await Task.Delay(30); // Simulate async operation
        
        var user = _users.FirstOrDefault(u => u.Id == id);
        return user != null ? MapToResponse(user) : null;
    }

    /// <summary>
    /// Gets a user by email
    /// </summary>
    public async Task<UserResponse?> GetUserByEmailAsync(string email)
    {
        _logger.LogInformation("Getting user by email: {Email}", email);
        
        await Task.Delay(30); // Simulate async operation
        
        var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        return user != null ? MapToResponse(user) : null;
    }

    /// <summary>
    /// Creates a new user
    /// </summary>
    public async Task<UserResponse> CreateUserAsync(CreateUserRequest request)
    {
        _logger.LogInformation("Creating user with email: {Email}", request.Email);
        
        // Validate email uniqueness
        if (_users.Any(u => u.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase)))
        {
            throw new ValidationException($"User with email '{request.Email}' already exists.");
        }

        await Task.Delay(100); // Simulate async operation

        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            DateOfBirth = request.DateOfBirth,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _users.Add(user);
        
        _logger.LogInformation("User created successfully with ID: {UserId}", user.Id);
        
        return MapToResponse(user);
    }

    /// <summary>
    /// Updates an existing user
    /// </summary>
    public async Task<UserResponse?> UpdateUserAsync(Guid id, UpdateUserRequest request)
    {
        _logger.LogInformation("Updating user with ID: {UserId}", id);
        
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            _logger.LogWarning("User not found with ID: {UserId}", id);
            return null;
        }

        // Validate email uniqueness if email is being changed
        if (!string.IsNullOrEmpty(request.Email) && 
            !request.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase) &&
            _users.Any(u => u.Id != id && u.Email.Equals(request.Email, StringComparison.OrdinalIgnoreCase)))
        {
            throw new ValidationException($"User with email '{request.Email}' already exists.");
        }

        await Task.Delay(80); // Simulate async operation

        // Update fields if provided
        if (!string.IsNullOrEmpty(request.FirstName))
            user.FirstName = request.FirstName;
        
        if (!string.IsNullOrEmpty(request.LastName))
            user.LastName = request.LastName;
        
        if (!string.IsNullOrEmpty(request.Email))
            user.Email = request.Email;
        
        if (request.PhoneNumber != null)
            user.PhoneNumber = request.PhoneNumber;
        
        if (request.DateOfBirth.HasValue)
            user.DateOfBirth = request.DateOfBirth;
        
        if (request.IsActive.HasValue)
            user.IsActive = request.IsActive.Value;

        user.UpdatedAt = DateTime.UtcNow;
        
        _logger.LogInformation("User updated successfully with ID: {UserId}", user.Id);
        
        return MapToResponse(user);
    }

    /// <summary>
    /// Deletes a user
    /// </summary>
    public async Task<bool> DeleteUserAsync(Guid id)
    {
        _logger.LogInformation("Deleting user with ID: {UserId}", id);
        
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            _logger.LogWarning("User not found with ID: {UserId}", id);
            return false;
        }

        await Task.Delay(50); // Simulate async operation

        _users.Remove(user);
        
        _logger.LogInformation("User deleted successfully with ID: {UserId}", id);
        
        return true;
    }

    /// <summary>
    /// Searches users by name or email
    /// </summary>
    public async Task<IEnumerable<UserResponse>> SearchUsersAsync(string searchTerm)
    {
        _logger.LogInformation("Searching users with term: {SearchTerm}", searchTerm);
        
        await Task.Delay(60); // Simulate async operation
        
        var searchTermLower = searchTerm.ToLowerInvariant();
        
        var matchingUsers = _users.Where(u => 
            u.FirstName.ToLowerInvariant().Contains(searchTermLower) ||
            u.LastName.ToLowerInvariant().Contains(searchTermLower) ||
            u.Email.ToLowerInvariant().Contains(searchTermLower) ||
            u.FullName.ToLowerInvariant().Contains(searchTermLower));

        return matchingUsers.Select(MapToResponse);
    }

    private static UserResponse MapToResponse(User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            DateOfBirth = user.DateOfBirth,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }

    private void SeedData()
    {
        var seedUsers = new List<User>
        {
            new User
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "+1-555-0123",
                DateOfBirth = new DateTime(1990, 5, 15),
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-30)
            },
            new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                PhoneNumber = "+1-555-0456",
                DateOfBirth = new DateTime(1985, 8, 22),
                IsActive = true,
                CreatedAt = DateTime.UtcNow.AddDays(-15)
            },
            new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Bob",
                LastName = "Johnson",
                Email = "bob.johnson@example.com",
                PhoneNumber = "+1-555-0789",
                DateOfBirth = new DateTime(1992, 12, 3),
                IsActive = false,
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                UpdatedAt = DateTime.UtcNow.AddDays(-1)
            }
        };

        _users.AddRange(seedUsers);
        _logger.LogInformation("Seeded {Count} users", seedUsers.Count);
    }
}
