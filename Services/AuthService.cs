using SecureFintechApi.Dtos;
using SecureFintechApi.Enums;
using SecureFintechApi.Models;

namespace SecureFintechApi.Services;

public class AuthService
{
    private readonly UserService _userService;
    private readonly JwtTokenService _jwtTokenService;

    public AuthService(
        UserService userService,
        JwtTokenService jwtTokenService
    )
    {
        _userService = userService;
        _jwtTokenService = jwtTokenService;
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
            Token = _jwtTokenService.GenerateToken(user),
            ExpiresAtUtc = _jwtTokenService.GetExpirationDate()
        };

        return OperationResult<LoginResponseDto>.Ok(
            response,
            "Login completed successfully."
        );
    }
}
