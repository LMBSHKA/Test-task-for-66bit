using Application.Interfaces.RepoIntrefaces;
using Data.DB;
using Models.Entities;

namespace Data.Repositories
{
    public class EquipmentTypeRepo : GenericRepo<EquipmentType>, IEquipmentTypeRepo
    {
        public EquipmentTypeRepo(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
