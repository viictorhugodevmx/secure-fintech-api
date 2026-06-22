using Microsoft.AspNetCore.Mvc;
using SecureFintechApi.Dtos;

namespace SecureFintechApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public ActionResult<ApiResponse<object>> GetHealth()
    {
        var health = new
        {
            status = "Healthy",
            service = "secure-fintech-api",
            checkedAtUtc = DateTime.UtcNow
        };

        return Ok(ApiResponse<object>.Ok(
            health,
            "API is healthy."
        ));
    }
}
