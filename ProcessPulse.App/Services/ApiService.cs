using Newtonsoft.Json;
using ProcessPulse.Class.ProcessPulse.Models;

namespace ProcessPulse.App.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string apiUrl = "https://processpulse.azurewebsites.net/api/Process/getProcesses";

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<ProcessInfo>> GetLastTenRecordsAsync(string terminalId)
        {
            var apiUrlForLastTenRecords = "https://processpulse.azurewebsites.net/api/Process/getLastTenRecords"; // Załóżmy, że masz taki endpoint
            var response = await _httpClient.GetAsync($"{apiUrlForLastTenRecords}?terminalId={terminalId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var processInfos = JsonConvert.DeserializeObject<List<ProcessInfo>>(content);
                return processInfos;
            }
            else
            {
                Console.WriteLine($"Błąd: {response.StatusCode}");
                return new List<ProcessInfo>();
            }
        }
        public async Task<ProcessInfo> GetProcessAsync(string terminalId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{apiUrl}?terminalId={terminalId}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var processInfo = JsonConvert.DeserializeObject<ProcessInfo>(content);
                    if (processInfo != null)
                    {
                        return processInfo;
                    }
                    else
                    {
                        Console.WriteLine("Deserializacja JSON nie powiodła się");
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine($"Błąd: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił wyjątek: {ex.Message}");
                return null;
            }
        }
    }
}
