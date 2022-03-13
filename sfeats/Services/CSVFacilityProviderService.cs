using CsvHelper;
using CsvHelper.Configuration;
using sfeats.Models;
using System.Globalization;
using System.Reflection;

namespace sfeats.Services
{
    public class CSVFacilityProviderService : IFacilityProviderService
    {
        public string ProviderName => "csv";

        public async Task<List<Facility>> GetFacilitiesAsync()
        {
            List<Facility> facilities = null;

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                NewLine = Environment.NewLine,
                PrepareHeaderForMatch = args => args.Header.ToLower(),
            };

            using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("sfeats.Mobile_Food_Facility_Permit.csv")))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                facilities = await Task.Run(()=> csv.GetRecords<Facility>().ToList());
            }

            return facilities;
        }
    }
}
