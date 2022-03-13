using BingMapsRESTToolkit;
using Newtonsoft.Json;
using System.ComponentModel;

namespace sfeats.Models
{
    public class Location
    {

        [DefaultValue(37.80499678)]
        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [DefaultValue(-122.409331696)]
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
