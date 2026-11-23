using Application.DTO.EquipmentItem;

namespace Application.Interfaces.ServiceInterface
{
    public interface IEquipmentItemService
    {
        public Task<Guid?> Add(CreateEquipmentItemDto createEquipmentItem, CancellationToken ct);
    }
}
