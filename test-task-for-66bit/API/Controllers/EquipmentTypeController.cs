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
    }
}
