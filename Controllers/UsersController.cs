using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecureFintechApi.Dtos;
using SecureFintechApi.Services;

namespace SecureFintechApi.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public ActionResult<ApiResponse<List<UserResponseDto>>> GetUsers()
    {
        List<UserResponseDto> users = _userService.GetUsers();

        return Ok(ApiResponse<List<UserResponseDto>>.Ok(
            users,
            "Users retrieved successfully."
        ));
    }

    [HttpGet("{email}")]
    [Authorize(Roles = "Admin,Analyst")]
    public ActionResult<ApiResponse<UserResponseDto>> GetUserByEmail(string email)
    {
        UserResponseDto? user = _userService.GetUserByEmail(email);

        if (user is null)
        {
            return NotFound(ApiResponse<UserResponseDto>.Fail(
                $"User {email} was not found."
            ));
        }

        return Ok(ApiResponse<UserResponseDto>.Ok(
            user,
            "User retrieved successfully."
        ));
    }
}
