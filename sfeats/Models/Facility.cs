using Newtonsoft.Json;

namespace sfeats.Models
{
    public class Facility
    {
        [JsonProperty("applicant")]
        public string Applicant { get; set; }

        [JsonProperty("facilitytype")]
        public string FacilityType { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        public Location Location => new Location() { Address = this.Address, Latitude = this.Latitude, Longitude = this.Longitude };
    }
}
