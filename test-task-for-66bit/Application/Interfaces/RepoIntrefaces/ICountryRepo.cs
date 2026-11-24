using Models.Entities;

namespace Application.Interfaces.RepoIntrefaces
{
    public interface ICountryRepo : IGenericRepo<Country>
    {
        public Task<Country?> FindNormalizedName(string normalizedName, CancellationToken ct);
    }
}
