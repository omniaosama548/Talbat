using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talbat.APIs.Errors;

namespace Talbat.APIs.Controllers
{
    [Route("errors/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ApiController : ControllerBase
    {

        public ActionResult Error(int code)
        {
            return NotFound(new ApiResponse(code));
        }
    }
}
