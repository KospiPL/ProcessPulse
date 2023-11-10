using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessPulse.ServerService.ProcessPulse.Service
{
    public class OsbService
    {
        private readonly HttpClient _httpClient;
        private readonly string _osbUrl = "https://osbicc01t.intercars.local:1116/SOALibrary/EnterpriseServiceLibrary/ProcessLevel/Service/SalesOrderFulfillment/V2";

        public OsbService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetSafoDataAsync()
        {
            var response = await _httpClient.GetAsync($"{_osbUrl}?systemId=PL");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetNavDataAsync()
        {
            var response = await _httpClient.GetAsync($"{_osbUrl}?systemId=PL_FLEET");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }



}
