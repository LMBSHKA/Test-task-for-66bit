using Application.Interfaces.RepoIntrefaces;
using Data.DB;
using Models.Entities;

namespace Data.Repositories
{
    public class EquipmentItemRepo : GenericRepo<EquipmentItem>, IEquipmentItemRepo
    {
        public EquipmentItemRepo(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
