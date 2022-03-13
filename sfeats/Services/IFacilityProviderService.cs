using sfeats.Models;

namespace sfeats.Services
{
    public interface IFacilityProviderService
    {
        string ProviderName { get; }

        public Task<List<Facility>> GetFacilitiesAsync();
    }
}
