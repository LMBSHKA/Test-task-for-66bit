using Application.DTO.EquipmentType;
using Models.Interfaces;

namespace Application.Interfaces.ServiceInterface
{
    public interface IGenericService<TEntity> where TEntity : class, ICatalogEntry
    {
        public Task<Guid> Add(CreateCatalogEntryDto createEquipmentType, CancellationToken ct);
        public Task<CatalogEntryDto?> Update(UpdateCatalogEntryDto updateEquipmentType, Guid equipmentTypeId,
            CancellationToken ct);
        public Task<bool> Delete(Guid equipmentTypeId, CancellationToken ct);
    }
}
