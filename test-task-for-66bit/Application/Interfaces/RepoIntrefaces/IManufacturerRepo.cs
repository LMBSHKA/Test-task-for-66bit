using Application.DTO.EquipmentItem.filters;
using Models.Entities;

namespace Application.Interfaces.RepoIntrefaces
{
    public interface IManufacturerRepo : IGenericRepo<Manufacturer>
    {
        Task<List<EquipmentItemFilterOptionDto>> GetAvailableManufacturersAsync(Guid? equipmentTypeId,
            CancellationToken ct);
    }
}
