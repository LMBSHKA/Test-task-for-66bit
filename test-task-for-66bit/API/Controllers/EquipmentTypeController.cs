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
        public async Task<IActionResult> Create(CreateEquipmentTypeDTO createEquipmentType, CancellationToken ct)
        {
            var id = await _equipmentTypeServce.Add(createEquipmentType, ct);

            return Ok(new { id });
        }

        [HttpPatch("update/{equipmentTypeId:guid}")]
        public async Task<IActionResult> Update(UpdateEquipmentTypeDTO updateEquipmentType, Guid equipmentTypeId,
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
            var deleted = await _equipmentTypeServce.Delete(equipmentTypeId, ct);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
