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

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateCatalogEntryDto createEquipmentType, CancellationToken ct)
        {
            var id = await _equipmentTypeServce.Add(createEquipmentType, ct);

            return Ok(new { id });
        }

        [HttpPatch("update/{equipmentTypeId:guid}")]
        public async Task<IActionResult> Update(UpdateCatalogEntryDto updateEquipmentType, Guid equipmentTypeId,
            CancellationToken ct)
        {
            var updatedEquipmentType = await _equipmentTypeServce.Update(updateEquipmentType, equipmentTypeId, ct);

            if (updatedEquipmentType == null)
                return NotFound();

            return Ok(updatedEquipmentType);
        }

        [HttpDelete("delete/{equipmentTypeId:guid}")]
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

        [HttpGet("all")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var allManufactures = await _equipmentTypeServce.GetAll(ct);

            return Ok(allManufactures);
        }
    }
}
