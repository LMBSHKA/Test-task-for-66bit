using Application.DTO.Country;
using Application.Interfaces.ServiceInterface;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountryController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateCountryDto createCountry, CancellationToken ct)
        {
            var countryId = await _countryService.Add(createCountry, ct);

            return Ok(countryId);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllCountry(CancellationToken ct)
        {
            var countries = await _countryService.GetAll(ct);

            return Ok(countries);
        }
    }
}
