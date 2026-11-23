using Application.DTO.EquipmentItem;
using Application.Interfaces.ServiceInterface;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class EquipmentItemController : ControllerBase
    {
        private readonly IEquipmentItemService _equipmentItemService;
        private readonly IValidator<CreateEquipmentItemDto> _createValidator;

        public EquipmentItemController(IEquipmentItemService equipmentItemService,
            IValidator<CreateEquipmentItemDto> createValidator)
        {
            _createValidator = createValidator;
            _equipmentItemService = equipmentItemService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateEquipmentItemDto createEquipmentItem, CancellationToken ct)
        {
            var validation = await _createValidator.ValidateAsync(createEquipmentItem, ct);

            if (!validation.IsValid)
            {
                foreach (var error in validation.Errors)
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

                return ValidationProblem(ModelState);
            }

            var equipmentItemId = await _equipmentItemService.Add(createEquipmentItem, ct);

            if (equipmentItemId == null)
                return BadRequest();

            return Ok(equipmentItemId);
        }
    }
}
