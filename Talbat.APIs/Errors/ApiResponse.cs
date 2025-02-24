namespace Talbat.APIs.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiResponse(int statuscode,string?message=null)
        {
            StatusCode = statuscode;
            Message = message ?? GetDefaultMessageForStatusCode(StatusCode);
        }

        private string? GetDefaultMessageForStatusCode(int? statusCode)
        {
            return StatusCode switch
            {
                400=>"Bad Requist",
                401=>"Not Authorized",
                404=>"Resource NotFound",
                500=>"Internal Server Error",
                _=>null
            };
        }
    }
}
