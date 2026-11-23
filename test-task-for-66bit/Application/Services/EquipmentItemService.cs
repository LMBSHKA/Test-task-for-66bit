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
            if (!await ReferencesExist(createEquipmentItem.EquipmentTypeId, createEquipmentItem.ManufacturerId,
                    createEquipmentItem.SupplyCountryId, ct))
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

        public async Task<UpdateEquipmentItemResult> Update(UpdateEquipmentItemDto updateEquipmentItem,
            Guid equipmentItemId, CancellationToken ct)
        {
            var equipmentItem = await _uow.EquipmentItems.FindAsync(equipmentItemId, ct);
            if (equipmentItem == null || equipmentItem.IsDeleted)
                return UpdateEquipmentItemResult.NotFound;

            if (!await ReferencesExist(updateEquipmentItem.EquipmentTypeId, updateEquipmentItem.ManufacturerId,
                    updateEquipmentItem.SupplyCountryId, ct))
                return UpdateEquipmentItemResult.InvalidReference;

            if (updateEquipmentItem.HasPhoto)
                equipmentItem.Photo = updateEquipmentItem.Photo;

            if (updateEquipmentItem.Model != null)
                equipmentItem.Model = updateEquipmentItem.Model.Trim();

            equipmentItem.EquipmentTypeId = updateEquipmentItem.EquipmentTypeId ?? equipmentItem.EquipmentTypeId;
            equipmentItem.ManufacturerId = updateEquipmentItem.ManufacturerId ?? equipmentItem.ManufacturerId;
            equipmentItem.SupplyCountryId = updateEquipmentItem.SupplyCountryId ?? equipmentItem.SupplyCountryId;
            equipmentItem.Price = updateEquipmentItem.Price ?? equipmentItem.Price;
            equipmentItem.UpdatedAt = DateTime.UtcNow;

            await _uow.SaveChangesAsync(ct);

            return UpdateEquipmentItemResult.Updated;
        }

        public async Task<bool> Delete(Guid equipmentItemId, CancellationToken ct)
        {
            var equipmentItem = await _uow.EquipmentItems.FindAsync(equipmentItemId, ct);

            if (equipmentItem == null || equipmentItem.IsDeleted)
                return false;

            var now = DateTime.UtcNow;
            equipmentItem.IsDeleted = true;
            equipmentItem.DeletedAt = now;
            equipmentItem.UpdatedAt = now;

            await _uow.SaveChangesAsync(ct);

            return true;
        }

        private async Task<bool> ReferencesExist(Guid? equipmentTypeId, Guid? manufacturerId, Guid? supplyCountryId,
            CancellationToken ct)
        {
            if (equipmentTypeId.HasValue &&
                !await _uow.EquipmentTypes.AnyAsync(x => x.Id == equipmentTypeId.Value, ct))
                return false;

            if (manufacturerId.HasValue &&
                !await _uow.Manufacturers.AnyAsync(x => x.Id == manufacturerId.Value, ct))
                return false;

            return !supplyCountryId.HasValue ||
                await _uow.Countries.AnyAsync(x => x.Id == supplyCountryId.Value, ct);
        }
    }
}
