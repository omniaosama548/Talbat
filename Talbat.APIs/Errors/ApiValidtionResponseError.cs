namespace Talbat.APIs.Errors
{
    public class ApiValidtionResponseError : ApiResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public ApiValidtionResponseError():base(400)
        {
            Errors = new List<string>();
        }
    }
}
