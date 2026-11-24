using Application.DTO.EquipmentItem.filters;
using Application.Interfaces.RepoIntrefaces;
using Data.DB;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Data.Repositories
{
    public class ManufacturerRepo : GenericRepo<Manufacturer>, IManufacturerRepo
    {
        public ManufacturerRepo(AppDbContext dbContext) : base(dbContext)
        {
        }

        public Task<List<EquipmentItemFilterOptionDto>> GetAvailableManufacturersAsync(Guid? equipmentTypeId,
            CancellationToken ct)
            => _dbContext.AsNoTracking()
                .Where(x => x.EquipmentItems.Any(item => !equipmentTypeId.HasValue
                    || item.EquipmentTypeId == equipmentTypeId))
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
