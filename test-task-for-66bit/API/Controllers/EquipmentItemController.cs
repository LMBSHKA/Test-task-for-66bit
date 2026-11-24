using API.Hubs;
using Application.DTO.EquipmentItem;
using Application.DTO.EquipmentItem.filters;
using Application.Interfaces.ServiceInterface;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class EquipmentItemController : ControllerBase
    {
        private readonly IEquipmentItemService _equipmentItemService;
        private readonly IValidator<CreateEquipmentItemDto> _createValidator;
        private readonly IValidator<UpdateEquipmentItemDto> _updateValidator;
        private readonly IHubContext<EquipmentHub> _equipmentHub;

        public EquipmentItemController(IEquipmentItemService equipmentItemService,
            IValidator<CreateEquipmentItemDto> createValidator,
            IValidator<UpdateEquipmentItemDto> updateValidator,
            IHubContext<EquipmentHub> equipmentHub)
        {
            _equipmentHub = equipmentHub;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _equipmentItemService = equipmentItemService;
        }

        /// <summary>Загружает фотографию оборудования.</summary>
        /// <remarks>
        /// Отправьте файл JPEG или PNG размером до 5 МБ включительно в поле <c>file</c>
        /// формы <c>multipart/form-data</c>. Полученный URL передайте в поле <c>photo</c>
        /// при создании или обновлении позиции склада.
        /// </remarks>
        /// <response code="200">Объект с относительным URL фотографии в поле <c>url</c>.</response>
        /// <response code="400">Файл отсутствует, пуст, превышает допустимый размер или имеет неподдерживаемый формат.</response>
        /// <response code="413">Размер всего HTTP-запроса превышает 6 МБ.</response>
        /// <response code="415">Запрос должен иметь тип <c>multipart/form-data</c>.</response>
        [HttpPost("upload-photo")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(6 * 1024 * 1024)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status413PayloadTooLarge)]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
        public async Task<IActionResult> UploadPhoto(IFormFile? file, [FromServices] IEquipmentPhotoService photoService,
            CancellationToken ct)
        {
            await using var content = file?.OpenReadStream();
            var (path, error) = await photoService.UploadAsync(content, file?.Length ?? 0, ct);
            if (error != null)
            {
                ModelState.AddModelError(nameof(file), error);
                return ValidationProblem(ModelState);
            }

            return Ok(new { url = Url.Content($"~/{path}") });
        }

        /// <summary>Создаёт позицию на складе.</summary>
        /// <param name="createEquipmentItem">Данные новой позиции. Тип техники, производитель и страна должны существовать в справочниках.</param>
        /// <param name="ct">Токен отмены запроса.</param>
        /// <response code="200">UUID созданной позиции.</response>
        /// <response code="400">Данные не прошли проверку либо указана отсутствующая запись справочника.</response>
        [HttpPost("create")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create(CreateEquipmentItemDto createEquipmentItem, CancellationToken ct)
        {
            var validation = await _createValidator.ValidateAsync(createEquipmentItem, ct);

            if (!validation.IsValid)
                return ValidationError(validation);

            var equipmentItemId = await _equipmentItemService.Add(createEquipmentItem, ct);

            if (equipmentItemId == null)
                return BadRequest();

            await _equipmentHub.Clients.All.SendAsync("EquipmentChanged", ct);

            return Ok(equipmentItemId);
        }

        /// <summary>Частично обновляет позицию склада.</summary>
        /// <remarks>
        /// Пропущенные поля сохраняют прежние значения. Передайте <c>"photo": null</c>,
        /// чтобы отвязать фотографию. Для остальных полей <c>null</c> сохраняет прежнее значение.
        /// </remarks>
        /// <param name="updateEquipmentItem">Поля позиции для обновления.</param>
        /// <param name="id">UUID позиции склада.</param>
        /// <param name="ct">Токен отмены запроса.</param>
        /// <response code="200">UUID обновлённой позиции.</response>
        /// <response code="400">Данные не прошли проверку либо указана отсутствующая запись справочника.</response>
        /// <response code="404">Позиция с таким идентификатором не найдена.</response>
        [HttpPatch("{id:guid}")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(UpdateEquipmentItemDto updateEquipmentItem, Guid id,
            CancellationToken ct)
        {
            var validation = await _updateValidator.ValidateAsync(updateEquipmentItem, ct);

            if (!validation.IsValid)
                return ValidationError(validation);

            var result = await _equipmentItemService.Update(updateEquipmentItem, id, ct);

            if (result == UpdateEquipmentItemResult.NotFound)
                return NotFound();

            if (result == UpdateEquipmentItemResult.InvalidReference)
                return BadRequest();

            await _equipmentHub.Clients.All.SendAsync("EquipmentChanged", ct);

            return Ok(id);
        }

        /// <summary>Удаляет позицию со склада.</summary>
        /// <param name="id">UUID позиции склада.</param>
        /// <param name="ct">Токен отмены запроса.</param>
        /// <response code="204">Позиция удалена.</response>
        /// <response code="404">Позиция с таким идентификатором не найдена.</response>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var deleted = await _equipmentItemService.Delete(id, ct);
            if (!deleted)
                return NotFound();

            await _equipmentHub.Clients.All.SendAsync("EquipmentChanged", ct);

            return NoContent();
        }

        /// <summary>Возвращает список оборудования с фильтрацией и пагинацией.</summary>
        /// <remarks>Первыми идут последние добавленные позиции. Без параметров возвращаются первые 20 позиций.</remarks>
        /// <response code="200">Страница списка, общее число подходящих позиций и варианты фильтрации по типу и производителю.</response>
        /// <response code="400">Параметры запроса не прошли проверку.</response>
        [HttpGet("all")]
        [ProducesResponseType(typeof(AllEquipmentItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] EquipmentItemFilterDto filter, CancellationToken ct)
        {
            var equipmentItems = await _equipmentItemService.GetAll(filter, ct);
            return Ok(equipmentItems);
        }

        private IActionResult ValidationError(ValidationResult validation)
        {
            foreach (var error in validation.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return ValidationProblem(ModelState);
        }
    }
}
