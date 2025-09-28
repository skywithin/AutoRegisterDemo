using Demo.Domain.Models;

namespace Demo.Domain.Abstractions;

public interface IUserDataProvider
{
    HashSet<User> Users { get; }
}

