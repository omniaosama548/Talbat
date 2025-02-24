using System.Text.Json;
using Talbat.APIs.Errors;

namespace Talbat.APIs.MiddleWares
{
    public class ExpectionMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExpectionMiddleWare> _logger;
        private readonly IHostEnvironment _env;

        public ExpectionMiddleWare(RequestDelegate Next,ILogger<ExpectionMiddleWare>Logger,IHostEnvironment env)
        {
            _next = Next;
            _logger = Logger;
            _env = env;
        }
        //invokeasync
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
               await _next.Invoke(context);
            }catch (Exception ex)
            {
                _logger.LogError(ex,ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;
                var Response = _env.IsDevelopment() ? new ApiExpectionResponse(500, ex.Message, ex.StackTrace.ToString()) : new ApiExpectionResponse(500);
                var options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                var JsonRes = JsonSerializer.Serialize(Response,options);
                context.Response.WriteAsync(JsonRes);
            }
          
        }
    }
}
