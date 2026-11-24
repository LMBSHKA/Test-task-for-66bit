using Application.DTO.Country;

namespace Application.Interfaces.ServiceInterface
{
    public interface ICountryService
    {
        public Task<Guid> Add(CreateCountryDto createCountry, CancellationToken ct);
    }
}
