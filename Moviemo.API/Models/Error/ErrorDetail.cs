using Newtonsoft.Json;

namespace Moviemo.API.Models
{
    public class ErrorDetail
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public override string ToString ()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
