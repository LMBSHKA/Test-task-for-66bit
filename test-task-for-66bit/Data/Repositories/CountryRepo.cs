using Application.Interfaces.RepoIntrefaces;
using Data.DB;
using Models.Entities;

namespace Data.Repositories
{
    public class CountryRepo : GenericRepo<Country>, ICountryRepo
    {
        public CountryRepo(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
