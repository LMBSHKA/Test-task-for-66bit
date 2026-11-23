using Application.DTO.EquipmentType;
using Application.Interfaces.RepoIntrefaces;
using Application.Interfaces.ServiceInterface;
using Models.Entities;

namespace Application.Services
{
    public class EquipmentTypeService : IEquipmentTypeServce
    {
        private readonly IWarehouseUnitOfWork _uow;
        public EquipmentTypeService(IWarehouseUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Guid> Add(CreateEquipmentTypeDTO createEquipmentType, CancellationToken ct)
        {
            var newEquipmentType = new EquipmentType
            {
                Id = new Guid(),
                Name = createEquipmentType.Name,
                Description = createEquipmentType.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _uow.EquipmentTypes.Add(newEquipmentType, ct);
            await _uow.SaveChangesAsync(ct);

            return newEquipmentType.Id;
        }

        public async Task<GetEquipmentTypeDTO?> Update(UpdateEquipmentTypeDTO updateEquipmentType, Guid equipmentTypeId,
            CancellationToken ct)
        {
            var equipmentType = await _uow.EquipmentTypes.GetById(equipmentTypeId, ct);
            if (equipmentType == null)
                return null;
            if (!string.IsNullOrWhiteSpace(updateEquipmentType.Name))
                equipmentType.Name = updateEquipmentType.Name;
            if (updateEquipmentType.Description != null)
                equipmentType.Description = updateEquipmentType.Description;

            equipmentType.UpdatedAt = DateTime.UtcNow;

            await _uow.SaveChangesAsync(ct);

            return new GetEquipmentTypeDTO
            {
                Id = equipmentType.Id,
                Name = equipmentType.Name,
                Description = equipmentType.Description
            };
        }

        public async Task<bool> Delete(Guid equipmentTypeId, CancellationToken ct)
        {
            var equipmentType = await _uow.EquipmentTypes.GetById(equipmentTypeId, ct);

            if (equipmentType == null)
                return false;

            _uow.EquipmentTypes.Delete(equipmentType);
            await _uow.SaveChangesAsync(ct);

            return true;
        }
    }
}
