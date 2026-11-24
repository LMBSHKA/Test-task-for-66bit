using Application.DTO.EquipmentItem;
using Application.DTO.EquipmentItem.filters;

namespace Application.Interfaces.ServiceInterface
{
    public interface IEquipmentItemService
    {
        public Task<Guid?> Add(CreateEquipmentItemDto createEquipmentItem, CancellationToken ct);
        public Task<UpdateEquipmentItemResult> Update(UpdateEquipmentItemDto updateEquipmentItem, Guid equipmentItemId,
            CancellationToken ct);
        public Task<bool> Delete(Guid equipmentItemId, CancellationToken ct);
        public Task<AllEquipmentItemDto> GetAll(EquipmentItemFilterDto filter, CancellationToken ct);
    }
}
