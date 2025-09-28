using AutoRegister.DI;
using Demo.Domain.Abstractions;
using Demo.Domain.Models;

namespace Demo.Infrastructure.Persistence;

[AutoRegister(Lifetime.Scoped, RegisterAs.Interface)]
internal sealed class UserDataProvider : IUserDataProvider
{
    private readonly HashSet<User> _users =
        new()
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

    public HashSet<User> Users => _users;
}

