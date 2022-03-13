using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using BingMapsRESTToolkit;
using sfeats.Models;

namespace sfeats.Services
{
    public class BingDistanceProviderService : IDistanceProviderService
    {
        private readonly IConfiguration _configuration;

        public BingDistanceProviderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<Result>> GetRoute(Options options, List<Facility> facilities)
        {
            var BingMapsKey = await GetBingMapsKey();

            TravelModeType travelMode;
            DistanceUnitType distanceUnit;

            if (!Enum.TryParse(options.TravelModeType, out travelMode))
            {
                throw new BadHttpRequestException($"Invalid TravelModeType '{options.TravelModeType}'");
            }

            if (!Enum.TryParse(options.DistanceUnitType, out distanceUnit))
            {
                throw new BadHttpRequestException($"Invalid DistanceUnitType '{options.DistanceUnitType}'");
            }

            var waypoints = new List<SimpleWaypoint>();
            var requests = new List<Task<Response>>();

            for (int i = 0; i < facilities.Count; i++)
            {
                var f = facilities[i];

                waypoints.Add(options.Origin.GetWaypoint());
                waypoints.Add(f.Location.GetWaypoint());

                if (waypoints.Count == 24 || i == facilities.Count - 1)
                {
                    var req = new RouteRequest()
                    {
                        Waypoints = waypoints,
                        BingMapsKey = BingMapsKey,
                        RouteOptions = new BingMapsRESTToolkit.RouteOptions()
                        {
                            DistanceUnits = distanceUnit,
                            TravelMode = travelMode
                        }
                    };
                    requests.Add(req.Execute());
                   
                    waypoints = new List<SimpleWaypoint>();
                }
            }

            int facility = 0;
            List<Result> results = new List<Result>();

            foreach (var o in requests)
            {
                var response = await o;

                if (response != null &&
                response.ResourceSets != null &&
                response.ResourceSets.Length > 0 &&
                response.ResourceSets[0].Resources != null &&
                response.ResourceSets[0].Resources.Length > 0)
                {
                    var route = ((BingMapsRESTToolkit.Route)response.ResourceSets[0].Resources[0]);

                    for (int i = 0; i < route.RouteLegs.Length; i = i + 2)
                    {
                        var leg = route.RouteLegs[i];

                        results.Add(new Result()
                        {
                            Destination = facilities[facility],
                            TravelDistance = leg.TravelDistance,
                            TravelDuration = leg.TravelDuration
                        });

                        facility++;
                    }
                }
            }

            return results;
        }

        private async Task<string> GetBingMapsKey() {
            SecretClientOptions options = new SecretClientOptions()
            {
                Retry =
                {
                    Delay= TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                }
            };
            var client = new SecretClient(new Uri(_configuration["AzureKeyVaultUrl"]), new DefaultAzureCredential(), options);

            KeyVaultSecret secret = await client.GetSecretAsync("BingMapsKey");

            return secret.Value;
        }
    }
}
