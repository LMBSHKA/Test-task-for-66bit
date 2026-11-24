using Application.DTO.Country;
using Application.Interfaces.RepoIntrefaces;
using Application.Interfaces.ServiceInterface;
using Models.Entities;

namespace Application.Services
{
    public class CountryService : ICountryService
    {
        private readonly IWarehouseUnitOfWork _uow;

        public CountryService(IWarehouseUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<Guid> Add(CreateCountryDto createCountry, CancellationToken ct)
        {
            var normalizedName = createCountry.Name.Trim().ToUpperInvariant();
            var country = await _uow.Countries.FindNormalizedName(normalizedName, ct);
            if (country == null)
            {
                var newCountry = new Country
                {
                    Id = Guid.NewGuid(),
                    Name = normalizedName
                };

                _uow.Countries.Add(newCountry);
                await _uow.SaveChangesAsync(ct);
                return newCountry.Id;
            }

            return country.Id;
        }
    }
}
