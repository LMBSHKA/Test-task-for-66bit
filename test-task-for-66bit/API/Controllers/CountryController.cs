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

        /// <summary>Добавляет страну в справочник.</summary>
        /// <remarks>
        /// Название сохраняется в верхнем регистре без пробелов по краям.
        /// Если страна с таким названием уже есть, запрос возвращает её идентификатор.
        /// </remarks>
        /// <param name="createCountry">Название страны.</param>
        /// <param name="ct">Токен отмены запроса.</param>
        /// <response code="200">UUID новой или существующей страны.</response>
        /// <response code="400">Название страны отсутствует или не прошло проверку.</response>
        [HttpPost("create")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(CreateCountryDto createCountry, CancellationToken ct)
        {
            var countryId = await _countryService.Add(createCountry, ct);

            return Ok(countryId);
        }

        /// <summary>Возвращает справочник стран.</summary>
        /// <response code="200">Список стран с идентификаторами и названиями.</response>
        [HttpGet("all")]
        [ProducesResponseType(typeof(AllCountryDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllCountry(CancellationToken ct)
        {
            var countries = await _countryService.GetAll(ct);

            return Ok(countries);
        }
    }
}
