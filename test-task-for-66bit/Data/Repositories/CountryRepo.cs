using Application.Interfaces.RepoIntrefaces;
using Data.DB;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Data.Repositories
{
    public class CountryRepo : GenericRepo<Country>, ICountryRepo
    {
        public CountryRepo(AppDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Country?> FindNormalizedName(string normalizedName, CancellationToken ct)
        {
            var country = await _dbContext.FirstOrDefaultAsync(x => x.Name == normalizedName, ct);

            return country;
        }
    }
}
