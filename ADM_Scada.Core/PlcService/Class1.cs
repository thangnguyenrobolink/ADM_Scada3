using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ADM_Scada.Core.PlcService
{
    public class webappdemoHttpClient
    {
        private string apiUrl = "http://localhost:8000/webapp";
        private readonly HttpClient _httpClient = new HttpClient();

        private async Task<HttpResponseMessage> MakeApiRequestAsync()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Process successful response (see step 5)
                }
                else
                {
                    // Handle error (see step 6)
                }

                return response;
            }
            catch (Exception ex)
            {
                // Handle exceptions (see step 6)
                return null;
            }
        }
    }
}
