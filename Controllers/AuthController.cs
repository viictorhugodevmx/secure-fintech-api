using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureFintechApi.Dtos;
using SecureFintechApi.Models;
using SecureFintechApi.Services;

namespace SecureFintechApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public ActionResult<ApiResponse<LoginResponseDto>> Login(
        [FromBody] LoginRequestDto request
    )
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return BadRequest(ApiResponse<LoginResponseDto>.Fail(
                "Email is required."
            ));
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(ApiResponse<LoginResponseDto>.Fail(
                "Password is required."
            ));
        }

        OperationResult<LoginResponseDto> result =
            _authService.Login(request);

        if (!result.Success)
        {
            return BadRequest(ApiResponse<LoginResponseDto>.Fail(
                result.Message
            ));
        }

        return Ok(ApiResponse<LoginResponseDto>.Ok(
            result.Data!,
            result.Message
        ));
    }

    [HttpGet("me")]
    [Authorize]
    public ActionResult<ApiResponse<CurrentUserResponseDto>> GetCurrentUser()
    {
        CurrentUserResponseDto currentUser = new CurrentUserResponseDto
        {
            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty,
            FullName = User.FindFirstValue(ClaimTypes.Name) ?? string.Empty,
            Email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty,
            Role = User.FindFirstValue(ClaimTypes.Role) ?? string.Empty
        };

        return Ok(ApiResponse<CurrentUserResponseDto>.Ok(
            currentUser,
            "Current user retrieved successfully."
        ));
    }
}
