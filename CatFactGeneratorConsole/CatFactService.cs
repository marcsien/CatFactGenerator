using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatFactGeneratorConsole
{
    public class CatFactService
    {
        private readonly HttpClient _httpClient;
        private readonly string _catFactApiUrl;

        public CatFactService(HttpClient httpClient, string catFactApiUrl)
        {
            _httpClient = httpClient;
            _catFactApiUrl = catFactApiUrl;
        }

        public async Task<string> GetRandomCatFact(int maxLength)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_catFactApiUrl + "/fact?max_length=" + maxLength.ToString());
            response.EnsureSuccessStatusCode();
            CatFact fact = await ParseReponse(response);
            return fact.fact;
        }

        public async Task<CatFact> ParseReponse(HttpResponseMessage httpResponse)
        {
            if (httpResponse.Content is object && httpResponse.Content.Headers.ContentType.MediaType == "application/json")
            {
                var contentStream = await httpResponse.Content.ReadAsStreamAsync();

                using var streamReader = new StreamReader(contentStream);
                using var jsonReader = new JsonTextReader(streamReader);

                JsonSerializer serializer = new JsonSerializer();

                return serializer.Deserialize<CatFact>(jsonReader); 
            }
            else
            {
                return null;
            }
        }
    }

    public class CatFact
    {
        public string fact { get; set; }
        public int length { get; set; }
    }
}
