using Microsoft.AspNetCore.Mvc;
using sfeats.Models;
using sfeats.Services;

namespace sfeats.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FacilitiesController : ControllerBase
    {

        private readonly IEnumerable<IFacilityProviderService> _facilityProviders;
        private readonly IDistanceProviderService _distanceService;
        private enum SortMode { Distance, Duration }

        public FacilitiesController(IEnumerable<IFacilityProviderService> facilityProviders, IDistanceProviderService distanceService)
        {
            _facilityProviders = facilityProviders;
            _distanceService = distanceService;
        }

        [HttpGet(Name = "GetFacilities")]
        public async Task<IActionResult> Get(
            string DataProvider = "csv",
            string TravelMode = "Walking",
            string Sort = "Distance",
            string Units = "Miles",
            int Count = 10,
            double latitude = 0.0,
            double longitude = 0.0,
            string address = "1355 Market St, San Francisco, CA 94103"
            )
        {

            SortMode sortMode;
            if (!Enum.TryParse(Sort, out sortMode))
            {
                return BadRequest($"Invalid parameter SortMode='{Sort}'");
            }


            Options callOptions = new Options()
            {
                DataProvider = DataProvider,
                TravelModeType = TravelMode,
                DistanceUnitType = Units,
                Count = Count,
                Origin = new Location()
                {
                    Latitude = latitude,
                    Longitude = longitude,
                    Address = address
                }
            };

            var facilityProvider = _facilityProviders.SingleOrDefault(o => o.ProviderName == callOptions.DataProvider);

            if (facilityProvider == null)
            {
                return BadRequest($"Invalid method '{callOptions.DataProvider}'");
            }


            List<Facility> facilities = await facilityProvider.GetFacilitiesAsync();
            facilities = facilities.Where(o => o.Status == "APPROVED").ToList();

            try
            {
                var results = await _distanceService.GetRoute(callOptions, facilities);


                if (sortMode == SortMode.Distance)
                {
                    results = results.OrderBy(o => o.TravelDistance).ToList();
                }
                if (sortMode == SortMode.Duration)
                {
                    results = results.OrderBy(o => o.TravelDuration).ToList();
                }

                results = results.GetRange(0, Math.Min(Count, results.Count));

                return Ok(results);
            }
            catch (BadHttpRequestException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}