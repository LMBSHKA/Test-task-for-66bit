using Models.Entities;

namespace Application.Interfaces.RepoIntrefaces
{
    public interface IEquipmentTypeRepo
    {
        public void Add(EquipmentType EquipmentType, CancellationToken ct);
        public Task<EquipmentType?> GetById(Guid equpmentTypeId, CancellationToken ct);
    }
}
