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
    }
}
