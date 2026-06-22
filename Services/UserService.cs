using SecureFintechApi.Dtos;
using SecureFintechApi.Enums;
using SecureFintechApi.Models;

namespace SecureFintechApi.Services;

public class UserService
{
    private readonly List<User> _users = new()
    {
        new User
        {
            Id = Guid.NewGuid(),
            FullName = "Víctor Hugo Segundo Aguilar",
            Email = "victor@securefintech.dev",
            Password = "Password123",
            Role = UserRole.Admin,
            Status = UserStatus.Active,
            CreatedAtUtc = DateTime.UtcNow.AddDays(-10)
        },
        new User
        {
            Id = Guid.NewGuid(),
            FullName = "Analista Fintech Demo",
            Email = "analyst@securefintech.dev",
            Password = "Password123",
            Role = UserRole.Analyst,
            Status = UserStatus.Active,
            CreatedAtUtc = DateTime.UtcNow.AddDays(-5)
        },
        new User
        {
            Id = Guid.NewGuid(),
            FullName = "Cliente Fintech Demo",
            Email = "customer@securefintech.dev",
            Password = "Password123",
            Role = UserRole.Customer,
            Status = UserStatus.Active,
            CreatedAtUtc = DateTime.UtcNow.AddDays(-2)
        },
        new User
        {
            Id = Guid.NewGuid(),
            FullName = "Cliente Bloqueado Demo",
            Email = "blocked@securefintech.dev",
            Password = "Password123",
            Role = UserRole.Customer,
            Status = UserStatus.Blocked,
            CreatedAtUtc = DateTime.UtcNow.AddDays(-1)
        }
    };

    public List<UserResponseDto> GetUsers()
    {
        return _users.Select(MapToResponseDto).ToList();
    }

    public UserResponseDto? GetUserByEmail(string email)
    {
        User? user = GetRawUserByEmail(email);

        if (user is null)
        {
            return null;
        }

        return MapToResponseDto(user);
    }

    public User? GetRawUserByEmail(string email)
    {
        return _users.FirstOrDefault(user =>
            user.Email.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase)
        );
    }

    private static UserResponseDto MapToResponseDto(User user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role,
            Status = user.Status,
            CreatedAtUtc = user.CreatedAtUtc
        };
    }
}
