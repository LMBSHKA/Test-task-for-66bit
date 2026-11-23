using Application.DTO.EquipmentItem;
using Application.Interfaces.RepoIntrefaces;
using Application.Interfaces.ServiceInterface;
using Models.Entities;

namespace Application.Services
{
    public class EquipmentItemService : IEquipmentItemService
    {
        private readonly IWarehouseUnitOfWork _uow;
        public EquipmentItemService(IWarehouseUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Guid?> Add(CreateEquipmentItemDto createEquipmentItem, CancellationToken ct)
        {
            var typeExists = await _uow.EquipmentTypes.AnyAsync(x => x.Id == createEquipmentItem.EquipmentTypeId, ct);
            var manufacturerExists = await _uow.Manufacturers.AnyAsync(x => x.Id == createEquipmentItem.ManufacturerId, ct);
            var countryExists = await _uow.Countries.AnyAsync(x => x.Id == createEquipmentItem.SupplyCountryId, ct);

            if (!typeExists || !manufacturerExists)
                return null;

            var newEquipmentItem = new EquipmentItem
            {
                Id = Guid.NewGuid(),
                Photo = createEquipmentItem.Photo,
                EquipmentTypeId = createEquipmentItem.EquipmentTypeId,
                ManufacturerId = createEquipmentItem.ManufacturerId,
                Model = createEquipmentItem.Model.Trim(),
                SupplyCountryId = createEquipmentItem.SupplyCountryId,
                Price = createEquipmentItem.Price,
                CreatedAt = DateTime.UtcNow
            };

            _uow.EquipmentItems.Add(newEquipmentItem);
            await _uow.SaveChangesAsync(ct);

            return newEquipmentItem.Id;
        }
    }
}
