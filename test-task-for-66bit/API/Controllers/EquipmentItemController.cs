using Application.DTO.EquipmentItem;
using Application.DTO.EquipmentItem.filters;
using Application.Interfaces.ServiceInterface;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class EquipmentItemController : ControllerBase
    {
        private readonly IEquipmentItemService _equipmentItemService;
        private readonly IValidator<CreateEquipmentItemDto> _createValidator;
        private readonly IValidator<UpdateEquipmentItemDto> _updateValidator;

        public EquipmentItemController(IEquipmentItemService equipmentItemService,
            IValidator<CreateEquipmentItemDto> createValidator,
            IValidator<UpdateEquipmentItemDto> updateValidator)
        {
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _equipmentItemService = equipmentItemService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateEquipmentItemDto createEquipmentItem, CancellationToken ct)
        {
            var validation = await _createValidator.ValidateAsync(createEquipmentItem, ct);

            if (!validation.IsValid)
                return ValidationError(validation);

            var equipmentItemId = await _equipmentItemService.Add(createEquipmentItem, ct);

            if (equipmentItemId == null)
                return BadRequest();

            return Ok(equipmentItemId);
        }

        [HttpPatch("{id:guid}")]
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

            return Ok(id);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            var deleted = await _equipmentItemService.Delete(id, ct);
            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpGet("all")]
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
