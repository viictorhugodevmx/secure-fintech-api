using SecureFintechApi.Dtos;
using SecureFintechApi.Enums;
using SecureFintechApi.Models;

namespace SecureFintechApi.Services;

public class AuthService
{
    private readonly UserService _userService;

    public AuthService(UserService userService)
    {
        _userService = userService;
    }

    public OperationResult<LoginResponseDto> Login(LoginRequestDto request)
    {
        User? user = _userService.GetRawUserByEmail(request.Email);

        if (user is null)
        {
            return OperationResult<LoginResponseDto>.Fail(
                "Invalid email or password."
            );
        }

        if (user.Password != request.Password)
        {
            return OperationResult<LoginResponseDto>.Fail(
                "Invalid email or password."
            );
        }

        if (user.Status != UserStatus.Active)
        {
            return OperationResult<LoginResponseDto>.Fail(
                $"User {user.Email} is not active."
            );
        }

        LoginResponseDto response = new LoginResponseDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role,
            Token = $"mock-token-{user.Role}-{Guid.NewGuid()}",
            ExpiresAtUtc = DateTime.UtcNow.AddHours(1)
        };

        return OperationResult<LoginResponseDto>.Ok(
            response,
            "Login completed successfully."
        );
    }
}
