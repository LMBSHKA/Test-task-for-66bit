using Application.Interfaces.Storage;

namespace Data.Storage
{
    public class LocalEquipmentPhotoStorage : IEquipmentPhotoStorage
    {
        private const string UploadsDirectory = "uploads";
        private readonly string _directoryPath;

        public LocalEquipmentPhotoStorage(string rootDirectory)
        {
            _directoryPath = Path.Combine(rootDirectory, UploadsDirectory);
        }

        public async Task<string> SaveAsync(Stream content, string extension, CancellationToken ct)
        {
            Directory.CreateDirectory(_directoryPath);

            var fileName = $"{Guid.NewGuid():N}{extension}";
            var filePath = Path.Combine(_directoryPath, fileName);
            var output = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None,
                bufferSize: 4096, useAsync: true);

            try
            {
                await using (output)
                    await content.CopyToAsync(output, ct);
            }
            catch
            {
                File.Delete(filePath);
                throw;
            }

            return $"{UploadsDirectory}/{fileName}";
        }
    }
}
