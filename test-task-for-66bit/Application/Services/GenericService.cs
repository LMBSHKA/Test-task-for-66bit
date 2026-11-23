using Application.DTO.CatalogEntry;
using Application.DTO.EquipmentType;
using Application.Interfaces.RepoIntrefaces;
using Application.Interfaces.ServiceInterface;
using Models.Interfaces;

namespace Application.Services
{
    public class GenericService<TEntity> : IGenericService<TEntity> where TEntity : class, ICatalogEntry, new()
    {
        private readonly IWarehouseUnitOfWork _uow;
        private readonly IGenericRepo<TEntity> _repo;

        public GenericService(IWarehouseUnitOfWork uow, IGenericRepo<TEntity> genericRepo)
        {
            _uow = uow;
            _repo = genericRepo;
        }

        public async Task<Guid> Add(CreateCatalogEntryDto createEquipmentType, CancellationToken ct)
        {
            var entity = new TEntity()
            {
                Id = Guid.NewGuid(),
                Name = createEquipmentType.Name,
                Description = createEquipmentType.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _repo.Add(entity);
            await _uow.SaveChangesAsync(ct);

            return entity.Id;
        }

        public async Task<CatalogEntryDto?> Update(UpdateCatalogEntryDto updateEquipmentType, Guid equipmentTypeId,
            CancellationToken ct)
        {
            var entity = await _repo.FindAsync(equipmentTypeId, ct);
            if (entity == null)
                return null;

            if (!string.IsNullOrWhiteSpace(updateEquipmentType.Name))
                entity.Name = updateEquipmentType.Name;
            if (updateEquipmentType.Description != null)
                entity.Description = updateEquipmentType.Description;

            entity.UpdatedAt = DateTime.UtcNow;

            await _uow.SaveChangesAsync(ct);

            return new CatalogEntryDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            };
        }

        public async Task<bool> Delete(Guid equipmentTypeId, CancellationToken ct)
        {
            var entity = await _repo.FindAsync(equipmentTypeId, ct);

            if (entity == null)
                return false;

            _repo.Remove(entity);
            await _uow.SaveChangesAsync(ct);

            return true;
        }

        public async Task<AllCatalogEntryDto> GetAll(CancellationToken ct)
        {
            var entities = await _repo.ListAsync(ct);

            return new AllCatalogEntryDto
            {
                ListCatalogEntry = entities.Select(entity => new CatalogEntryDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description
                }).ToList()
            };
        }
    }
}
