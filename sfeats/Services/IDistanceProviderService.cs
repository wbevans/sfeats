using sfeats.Models;

namespace sfeats.Services
{
    public interface IDistanceProviderService
    {
        public Task<List<Result>> GetRoute(Options options, List<Facility> facilities);
    }
}
