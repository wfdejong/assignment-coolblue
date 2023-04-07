using System.Net;

namespace Insurance.Api.Dto
{
    public class ApiDto<T>
    {
        public HttpStatusCode StatusCode { get; }
        public bool IsSuccessStatusCode { get; }

        public T Result { get; set; }

        public ApiDto(HttpStatusCode statusCode, bool isSuccessStatusCode)
        {
            StatusCode= statusCode;
            IsSuccessStatusCode= isSuccessStatusCode;
        }
    }
}
