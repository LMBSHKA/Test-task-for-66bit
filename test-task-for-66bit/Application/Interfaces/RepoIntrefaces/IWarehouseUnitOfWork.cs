namespace Application.Interfaces.RepoIntrefaces
{
    public interface IWarehouseUnitOfWork
    {
        IEquipmentTypeRepo EquipmentTypes { get; }
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
