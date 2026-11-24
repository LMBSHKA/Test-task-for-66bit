using Application.DTO.CatalogEntry;
using Application.DTO.EquipmentType;
using Application.Interfaces.ServiceInterface;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class EquipmentTypeController : ControllerBase
    {
        private readonly IEquipmentTypeServce _equipmentTypeServce;

        public EquipmentTypeController(
            IEquipmentTypeServce equipmentTypeService)
        {
            _equipmentTypeServce = equipmentTypeService;
        }

        /// <summary>Добавляет тип техники в справочник.</summary>
        /// <param name="createEquipmentType">Название и необязательное описание типа техники.</param>
        /// <param name="ct">Токен отмены запроса.</param>
        /// <response code="200">Объект с UUID созданного типа техники в поле <c>id</c>.</response>
        /// <response code="400">Данные не прошли проверку.</response>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(CreateCatalogEntryDto createEquipmentType, CancellationToken ct)
        {
            var id = await _equipmentTypeServce.Add(createEquipmentType, ct);

            return Ok(new { id });
        }

        /// <summary>Частично обновляет тип техники.</summary>
        /// <remarks>Пропущенные поля и поля со значением <c>null</c> сохраняют прежние значения.</remarks>
        /// <param name="updateEquipmentType">Поля типа техники для обновления.</param>
        /// <param name="equipmentTypeId">UUID типа техники.</param>
        /// <param name="ct">Токен отмены запроса.</param>
        /// <response code="200">Обновлённая запись справочника.</response>
        /// <response code="400">Данные не прошли проверку.</response>
        /// <response code="404">Тип техники с таким идентификатором не найден.</response>
        [HttpPatch("update/{equipmentTypeId:guid}")]
        [ProducesResponseType(typeof(CatalogEntryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(UpdateCatalogEntryDto updateEquipmentType, Guid equipmentTypeId,
            CancellationToken ct)
        {
            var updatedEquipmentType = await _equipmentTypeServce.Update(updateEquipmentType, equipmentTypeId, ct);

            if (updatedEquipmentType == null)
                return NotFound();

            return Ok(updatedEquipmentType);
        }

        /// <summary>Удаляет тип техники из справочника.</summary>
        /// <param name="equipmentTypeId">UUID типа техники.</param>
        /// <param name="ct">Токен отмены запроса.</param>
        /// <response code="204">Тип техники удалён.</response>
        /// <response code="404">Тип техники с таким идентификатором не найден.</response>
        /// <response code="409">На тип техники ссылаются позиции склада.</response>
        [HttpDelete("delete/{equipmentTypeId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Delete(Guid equipmentTypeId, CancellationToken ct)
        {
            var result = await _equipmentTypeServce.Delete(equipmentTypeId, ct);

            if (result == DeleteCatalogEntryResult.NotFound)
                return NotFound();

            if (result == DeleteCatalogEntryResult.InUse)
                return Problem(statusCode: StatusCodes.Status409Conflict,
                    title: "Тип техники используется в позициях склада.",
                    detail: "Сначала измените или удалите связанные позиции.");

            return NoContent();
        }

        /// <summary>Возвращает справочник типов техники.</summary>
        /// <response code="200">Список типов техники.</response>
        [HttpGet("all")]
        [ProducesResponseType(typeof(AllCatalogEntryDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var allManufactures = await _equipmentTypeServce.GetAll(ct);

            return Ok(allManufactures);
        }
    }
}
