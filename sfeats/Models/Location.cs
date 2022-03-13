using BingMapsRESTToolkit;
using Newtonsoft.Json;

namespace sfeats.Models
{
    public class Location
    {

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("human_address")]
        public string Address { get; set; }

        internal SimpleWaypoint GetWaypoint()
        {
            if (Longitude == 0 || Longitude == 0)
            {
                string fulladdress = Address;
                if (!fulladdress.ToLower().Contains("san francisco"))
                {
                    fulladdress += ", San Francisco, CA";
                }
                return new SimpleWaypoint(fulladdress);
            }
            else
            {
                return new SimpleWaypoint(Latitude, Longitude);
            }
        }
    }
}
