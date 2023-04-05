using Insurance.Api.Dto;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using System.Net.Http.Json;

namespace Insurance.Api.Infrasctructure
{
    public abstract class BaseApi
    {
        private readonly string _apiAddress;

        public BaseApi(string apiAddress)
        {
            _apiAddress = apiAddress;
        }
        protected async Task<Dto<T>> Get<T>(string path)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(_apiAddress);

                HttpResponseMessage response = await client.GetAsync(path);

                Dto<T> dto = (Dto<T>)Activator.CreateInstance(typeof(Dto<T>), response.StatusCode, response.IsSuccessStatusCode);

                if (response.IsSuccessStatusCode)
                {
                    dto.Result = await response.Content.ReadFromJsonAsync<T>();
                }

                return dto;
            }
        }
    }
}
