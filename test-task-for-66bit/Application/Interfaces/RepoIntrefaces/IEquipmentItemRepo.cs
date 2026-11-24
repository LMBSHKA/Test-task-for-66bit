using Application.DTO.EquipmentItem;
using Application.DTO.EquipmentItem.filters;
using Models.Entities;

namespace Application.Interfaces.RepoIntrefaces
{
    public interface IEquipmentItemRepo : IGenericRepo<EquipmentItem>
    {
        Task<(List<EquipmentItemDto> EquipmentItems, int TotalCount)> ListAsync(EquipmentItemFilterDto filter,
            CancellationToken ct);
    }
}
