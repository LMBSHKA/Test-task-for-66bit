using Application.DTO.EquipmentItem;

namespace Application.Interfaces.ServiceInterface
{
    public interface IEquipmentItemService
    {
        public Task<Guid?> Add(CreateEquipmentItemDto createEquipmentItem, CancellationToken ct);
        public Task<UpdateEquipmentItemResult> Update(UpdateEquipmentItemDto updateEquipmentItem, Guid equipmentItemId,
            CancellationToken ct);
        public Task<bool> Delete(Guid equipmentItemId, CancellationToken ct);
    }
}
