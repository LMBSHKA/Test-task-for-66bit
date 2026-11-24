using Application.DTO.EquipmentItem.filters;
using Models.Entities;

namespace Application.Interfaces.RepoIntrefaces
{
    public interface IEquipmentTypeRepo : IGenericRepo<EquipmentType>
    {
        Task<List<EquipmentItemFilterOptionDto>> GetAvailableEquipmentTypesAsync(Guid? manufacturerId,
            CancellationToken ct);
    }
}
