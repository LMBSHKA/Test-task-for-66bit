using Application.DTO.EquipmentItem;
using Application.DTO.EquipmentItem.filters;
using Application.Interfaces.RepoIntrefaces;
using Data.DB;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Data.Repositories
{
    public class EquipmentItemRepo : GenericRepo<EquipmentItem>, IEquipmentItemRepo
    {
        public EquipmentItemRepo(AppDbContext dbContext) : base(dbContext)
        {
        }

        // Заменяем %, _ и \, чтобы ILIKE искал введённые символы буквально
        public async Task<(List<EquipmentItemDto> EquipmentItems, int TotalCount)> ListAsync(
            EquipmentItemFilterDto filter, CancellationToken ct)
        {
            var query = _dbContext.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var search = filter.Search.Trim()
                    .Replace("\\", "\\\\")
                    .Replace("%", "\\%")
                    .Replace("_", "\\_");
                var pattern = $"%{search}%";

                query = query.Where(x => EF.Functions.ILike(x.Model, pattern, "\\")
                    || EF.Functions.ILike(x.EquipmentType.Name, pattern, "\\")
                    || EF.Functions.ILike(x.Manufacturer.Name, pattern, "\\"));
            }

            if (filter.EquipmentTypeId is Guid equipmentTypeId)
                query = query.Where(x => x.EquipmentTypeId == equipmentTypeId);
            if (filter.ManufacturerId is Guid manufacturerId)
                query = query.Where(x => x.ManufacturerId == manufacturerId);
            if (filter.MinPrice is decimal minPrice)
                query = query.Where(x => x.Price >= minPrice);
            if (filter.MaxPrice is decimal maxPrice)
                query = query.Where(x => x.Price <= maxPrice);

            var totalCount = await query.CountAsync(ct);
            var offset = (filter.Page - 1L) * filter.PageSize;
            if (offset >= totalCount)
                return (new List<EquipmentItemDto>(), totalCount);

            var equipmentItems = await query
                .OrderByDescending(x => x.CreatedAt)
                .ThenBy(x => x.Id)
                .Skip((int)offset)
                .Take(filter.PageSize)
                .Select(x => new EquipmentItemDto
                {
                    Id = x.Id,
                    Photo = x.Photo,
                    EquipmentTypeName = x.EquipmentType.Name,
                    EquipmentTypeId = x.EquipmentTypeId,
                    ManufacturerName = x.Manufacturer.Name,
                    ManufacturerId = x.ManufacturerId,
                    Model = x.Model,
                    SupplyCountryName = x.SupplyCountry.Name,
                    SupplyCountryId = x.SupplyCountryId,
                    Price = x.Price,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .ToListAsync(ct);

            return (equipmentItems, totalCount);
        }
    }
}
