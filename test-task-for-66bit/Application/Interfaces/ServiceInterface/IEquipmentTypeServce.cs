using Application.DTO.EquipmentType;

namespace Application.Interfaces.ServiceInterface
{
    public interface IEquipmentTypeServce
    {
        public Task<Guid> Add(CreateEquipmentTypeDTO createEquipmentType, CancellationToken ct);
    }
}
