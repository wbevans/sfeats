using Newtonsoft.Json;
using sfeats.Models;

namespace sfeats.Services
{
    public class DataSFFacilityProviderService : IFacilityProviderService
    {
        public string ProviderName => "datasf";

        public async Task<List<Facility>> GetFacilitiesAsync()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://data.sfgov.org");

            var streamTask = await client.GetStreamAsync("resource/rqzj-sfat.json");

            using (StreamReader reader = new StreamReader(streamTask))
            {
                string json = await reader.ReadToEndAsync();
                return JsonConvert.DeserializeObject<List<Facility>>(json);
            }
        }
    }
}
