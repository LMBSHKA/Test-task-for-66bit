using Application.DTO.EquipmentItem.filters;
using Application.Interfaces.RepoIntrefaces;
using Data.DB;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Data.Repositories
{
    public class EquipmentTypeRepo : GenericRepo<EquipmentType>, IEquipmentTypeRepo
    {
        public EquipmentTypeRepo(AppDbContext dbContext) : base(dbContext)
        {
        }

        public Task<List<EquipmentItemFilterOptionDto>> GetAvailableEquipmentTypesAsync(Guid? manufacturerId,
            CancellationToken ct)
            => _dbContext.AsNoTracking()
                .Where(x => x.EquipmentItems.Any(item => !manufacturerId.HasValue
                    || item.ManufacturerId == manufacturerId))
                .OrderBy(x => x.Name)
                .ThenBy(x => x.Id)
                .Select(x => new EquipmentItemFilterOptionDto
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync(ct);
    }
}
