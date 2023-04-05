using System.Net;

namespace Insurance.Api.Dto
{
    public class Dto<T>
    {
        public HttpStatusCode StatusCode { get; }
        public bool IsSuccessStatusCode { get; }

        public T Result { get; set; }

        public Dto(HttpStatusCode statusCode, bool isSuccessStatusCode)
        {
            StatusCode= statusCode;
            IsSuccessStatusCode= isSuccessStatusCode;
        }
    }
}
