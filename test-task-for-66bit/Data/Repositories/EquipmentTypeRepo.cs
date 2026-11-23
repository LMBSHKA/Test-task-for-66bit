using Application.Interfaces.RepoIntrefaces;
using Data.DB;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Data.Repositories
{
    public class EquipmentTypeRepo : IEquipmentTypeRepo
    {
        private readonly AppDbContext _dbContext;
        public EquipmentTypeRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(EquipmentType equipmentType, CancellationToken ct)
        {
            _dbContext.EquipmentTypes.Add(equipmentType);
        }

        public async Task<EquipmentType?> GetById(Guid id, CancellationToken ct)
        {
            return await _dbContext.EquipmentTypes.FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public void Delete(EquipmentType equipmentType)
        {
            _dbContext.EquipmentTypes.Remove(equipmentType);
        }
    }
}
