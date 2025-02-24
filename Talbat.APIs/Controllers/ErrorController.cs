using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talbat.APIs.Errors;

namespace Talbat.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        public ActionResult Error(int code)
        {
            return NotFound(new ApiResponse(code));
        }
    }
}
