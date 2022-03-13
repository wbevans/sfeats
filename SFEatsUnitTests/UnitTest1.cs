using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using sfeats.Models;
using sfeats.Services;
using System;
using System.Collections.Generic;

namespace SFEatsUnitTests
{
    [TestClass]
    public class DistanceProviderTests
    {
        [TestMethod]
        public void BingTest()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"AzureKeyVaultUrl", "https://wbemykeyvault.vault.azure.net/"}
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var bing = new BingDistanceProviderService(configuration);

            Options callOptions = new Options()
            {

                TravelModeType =  "Walking",
                DistanceUnitType = "Miles",
                Count = 10,
                Origin = new Location()
                {
                    Latitude = 0.0,
                    Longitude = 0.0,
                    Address = "2640 Steiner St, San Francisco, CA 94115"
                }
            };

            var facilities = new List<Facility>();
            facilities.Add(new Facility() { Address = "1363 Divisadero St, San Francisco, CA 94115" });
            facilities.Add(new Facility() { Address = "625 Monterey Blvd, San Francisco, CA 94127" });
            facilities.Add(new Facility() { Address = "3 Masonic Ave, San Francisco, CA 94118" });
            
            var results = bing.GetRoute(callOptions, facilities).Result;

            Assert.AreEqual(1.0, Math.Round(results[0].TravelDistance,1));
            Assert.AreEqual(5.3, Math.Round(results[1].TravelDistance, 1));
            Assert.AreEqual(1.2, Math.Round(results[2].TravelDistance, 1));
        }
    }
}