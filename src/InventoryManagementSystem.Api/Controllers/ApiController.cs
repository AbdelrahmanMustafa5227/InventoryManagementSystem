using InventoryManagementSystem.Application.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Api.Controllers
{

    [ApiController]
    public class ApiController : ControllerBase
    {
        protected IActionResult ToProblemDetails(Error error)
        {
            return Problem(
                statusCode: (int)error.StatusCode,
                title: error.Title,
                detail: error.Message
                );
        }
    }
}
