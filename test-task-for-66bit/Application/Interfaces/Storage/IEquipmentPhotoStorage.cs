namespace Application.Interfaces.Storage
{
    public interface IEquipmentPhotoStorage
    {
        Task<string> SaveAsync(Stream content, string extension, CancellationToken ct);
    }
}
