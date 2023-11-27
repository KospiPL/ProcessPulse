using Azure;
using Newtonsoft.Json;
using ProcessPulse.Class.ProcessPulse.Models;
using System.Net.Http.Json;

namespace ProcessPulse.App.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string baseapiUrl = "https://processpulse.azurewebsites.net/api";

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<ProcessInfo>> GetLastTenRecordsAsync(string terminalId)
        {
            var apiUrlForLastTenRecords = "https://processpulse.azurewebsites.net/api/Process/getLastTenRecords"; 
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
        public async Task AddTerminalAsync(TerminalMapping terminal)
        {
            var response = await _httpClient.PostAsJsonAsync($"{baseapiUrl}/Terminal", terminal);
            response.EnsureSuccessStatusCode();
        }
        public async Task UpdateTerminalAsync(TerminalMapping terminal)
        {
            var response = await _httpClient.PutAsJsonAsync($"{baseapiUrl}/Terminal/{terminal.Id}", terminal);
            response.EnsureSuccessStatusCode();
        }
        public async Task DeleteTerminalAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{baseapiUrl}/Terminal/{id}");
            response.EnsureSuccessStatusCode();
        }
        public async Task<List<TerminalMapping>> GetTerminalsAsync()
        {
            var response = await _httpClient.GetAsync($"{baseapiUrl}/Terminal");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<TerminalMapping>>(content);
            }
            return new List<TerminalMapping>();
        }
        public async Task<TerminalMapping> GetTerminalAsync(int id)
        {
            var response = await _httpClient.GetAsync($"{baseapiUrl}/Terminal/{id}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TerminalMapping>(content);
            }
            return null;
        }


        public async Task<ProcessInfo> GetProcessAsync(string terminalId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{baseapiUrl}?terminalId={terminalId}");
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
        public async Task<List<FlotaModel>> GetStatusDataAsync()
        {
            
            var response = await _httpClient.GetAsync("https://processpulse.azurewebsites.net/api/Flota");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<FlotaModel>>(content);
            }
            return new List<FlotaModel>(); 
        }
        public async Task<List<SafoModel>> GetSafoDataAsync()
        {
            var response = await _httpClient.GetAsync("https://processpulse.azurewebsites.net/api/Safo");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<SafoModel>>(content);
            }
            return new List<SafoModel>();
        }

    }
}
