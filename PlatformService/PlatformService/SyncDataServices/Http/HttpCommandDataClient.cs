using PlatformService.Dtos;
using System.Text;
using System.Text.Json;
using System.Text.Unicode;

namespace PlatformService.SyncDataServices.Http
{
    public class HttpCommandDataClient : ICommandDataClient
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _configuration;

        public HttpCommandDataClient(HttpClient http, IConfiguration configuration)
        {
           _http = http;
           _configuration = configuration;
        }
        public async Task SendPlatformsToClient(PlatformReadDto platform)
        {
            var httpContent = new StringContent(
                              JsonSerializer.Serialize(platform),
                              Encoding.UTF8,
                              "application/json"
                              );
            var response = await _http.PostAsync(_configuration["CommandService"], httpContent);

            if (response.IsSuccessStatusCode)
                Console.WriteLine($"response is ok! {response.Content}");

            else
                Console.WriteLine("Response is not ok!");
        }
    }
}
