using Application.DTO.CatalogEntry;
using Application.DTO.EquipmentType;
using Application.Interfaces.ServiceInterface;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class ManufacturerController : ControllerBase
    {
        private readonly IManufacturerService _manufacturerService;

        public ManufacturerController(IManufacturerService manufacturerService)
        {
            _manufacturerService = manufacturerService;
        }

        /// <summary>Добавляет производителя в справочник.</summary>
        /// <param name="createManufacturer">Название и необязательное описание производителя.</param>
        /// <param name="ct">Токен отмены запроса.</param>
        /// <response code="200">Объект с UUID созданного производителя в поле <c>id</c>.</response>
        /// <response code="400">Данные не прошли проверку.</response>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(CreateCatalogEntryDto createManufacturer, CancellationToken ct)
        {
            var id = await _manufacturerService.Add(createManufacturer, ct);

            return Ok(new { id });
        }

        /// <summary>Частично обновляет производителя.</summary>
        /// <remarks>Пропущенные поля и поля со значением <c>null</c> сохраняют прежние значения.</remarks>
        /// <param name="updateEquipmentType">Поля производителя для обновления.</param>
        /// <param name="manufacturerId">UUID производителя.</param>
        /// <param name="ct">Токен отмены запроса.</param>
        /// <response code="200">Обновлённая запись справочника.</response>
        /// <response code="400">Данные не прошли проверку.</response>
        /// <response code="404">Производитель с таким идентификатором не найден.</response>
        [HttpPatch("update/{manufacturerId:guid}")]
        [ProducesResponseType(typeof(CatalogEntryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(UpdateCatalogEntryDto updateEquipmentType, Guid manufacturerId,
            CancellationToken ct)
        {
            var updatedManufacturer = await _manufacturerService.Update(updateEquipmentType, manufacturerId, ct);

            if (updatedManufacturer == null)
                return NotFound();

            return Ok(updatedManufacturer);
        }

        /// <summary>Удаляет производителя из справочника.</summary>
        /// <param name="manufacturerId">UUID производителя.</param>
        /// <param name="ct">Токен отмены запроса.</param>
        /// <response code="204">Производитель удалён.</response>
        /// <response code="404">Производитель с таким идентификатором не найден.</response>
        /// <response code="409">На производителя ссылаются позиции склада.</response>
        [HttpDelete("delete/{manufacturerId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete(Guid manufacturerId, CancellationToken ct)
        {
            var result = await _manufacturerService.Delete(manufacturerId, ct);

            if (result == DeleteCatalogEntryResult.NotFound)
                return NotFound();

            if (result == DeleteCatalogEntryResult.InUse)
                return Problem(statusCode: StatusCodes.Status409Conflict,
                    title: "Производитель используется в позициях склада.",
                    detail: "Сначала измените или удалите связанные позиции.");

            return NoContent();
        }

        /// <summary>Возвращает справочник производителей.</summary>
        /// <response code="200">Список производителей.</response>
        [HttpGet("all")]
        [ProducesResponseType(typeof(AllCatalogEntryDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var allManufactures = await _manufacturerService.GetAll(ct);

            return Ok(allManufactures);
        }
    }
}
