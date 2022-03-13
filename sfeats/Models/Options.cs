using BingMapsRESTToolkit;
using System.ComponentModel;

namespace sfeats.Models
{
    public class Options
    {
        [DefaultValue("datasf")]
        public string DataProvider { get; set; }

        public Location Origin { get; set; }

        [DefaultValue("Walking")]
        public string TravelModeType { get; set; }

        [DefaultValue("Miles")]
        public string DistanceUnitType { get; set; }
        
        [DefaultValue(10)]
        public int Count { get; set; }

    }
}
