using Application.Interfaces.RepoIntrefaces;
using Data.DB;
using Models.Entities;

namespace Data.Repositories
{
    public class ManufacturerRepo : GenericRepo<Manufacturer>, IManufacturerRepo
    {
        public ManufacturerRepo(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
