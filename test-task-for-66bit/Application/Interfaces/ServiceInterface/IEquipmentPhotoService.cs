namespace Application.Interfaces.ServiceInterface
{
    public interface IEquipmentPhotoService
    {
        // The caller owns the seekable stream and supplies the length starting at its current position.
        Task<(string? RelativePath, string? Error)> UploadAsync(Stream? content, long length, CancellationToken ct);
    }
}
