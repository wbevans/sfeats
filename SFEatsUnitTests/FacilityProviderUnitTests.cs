using Microsoft.VisualStudio.TestTools.UnitTesting;
using sfeats.Services;

namespace SFEatsUnitTests
{
    [TestClass]
    public class FacilityProviderUnitTests
    {
        [TestMethod]
        public void CsvTest()
        {
            CSVFacilityProviderService service = new CSVFacilityProviderService();

            var results = service.GetFacilitiesAsync().Result;

            Assert.AreEqual(619, results.Count);
            Assert.AreEqual("Pipo's Grill", results[0].Applicant);
        }

        [TestMethod]
        public void DataSfTest()
        {
            DataSFFacilityProviderService service = new DataSFFacilityProviderService();
            var results = service.GetFacilitiesAsync().Result;

            Assert.IsTrue(results.Count > 450);
        }
    }
}