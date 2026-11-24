using Application.Interfaces.ServiceInterface;
using Application.Interfaces.Storage;

namespace Application.Services
{
    public class EquipmentPhotoService : IEquipmentPhotoService
    {
        private static readonly byte[] PngSignature = { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A };
        private static readonly byte[] JpegSignature = { 0xFF, 0xD8, 0xFF };

        private readonly IEquipmentPhotoStorage _storage;

        public EquipmentPhotoService(IEquipmentPhotoStorage storage)
        {
            _storage = storage;
        }

        public async Task<(string? RelativePath, string? Error)> UploadAsync(Stream? content, long length,
            CancellationToken ct)
        {
            if (content == null || length <= 0)
                return (null, "Выберите файл фотографии.");

            if (length > 5 * 1024 * 1024)
                return (null, "Размер фотографии не должен превышать 5 МБ.");

            var startPosition = content.Position;
            var header = new byte[PngSignature.Length];
            var bytesRead = 0;
            while (bytesRead < header.Length)
            {
                var read = await content.ReadAsync(header.AsMemory(bytesRead), ct);
                if (read == 0)
                    break;

                bytesRead += read;
            }
            content.Position = startPosition;

            string extension;
            if (header.AsSpan(0, bytesRead).StartsWith(PngSignature))
                extension = ".png";
            else if (header.AsSpan(0, bytesRead).StartsWith(JpegSignature))
                extension = ".jpg";
            else
                return (null, "Поддерживаются фотографии в формате JPEG или PNG.");

            var relativePath = await _storage.SaveAsync(content, extension, ct);
            return (relativePath, null);
        }
    }
}
