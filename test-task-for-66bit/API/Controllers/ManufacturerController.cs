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

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateCatalogEntryDto createManufacturer, CancellationToken ct)
        {
            var id = await _manufacturerService.Add(createManufacturer, ct);

            return Ok(new { id });
        }

        [HttpPatch("update/{manufacturerId:guid}")]
        public async Task<IActionResult> Update(UpdateCatalogEntryDto updateEquipmentType, Guid manufacturerId,
            CancellationToken ct)
        {
            var updatedManufacturer = await _manufacturerService.Update(updateEquipmentType, manufacturerId, ct);

            if (updatedManufacturer == null)
                return NotFound();

            return Ok(updatedManufacturer);
        }

        [HttpDelete("delete/{manufacturerId:guid}")]
        public async Task<IActionResult> Delete(Guid manufacturerId, CancellationToken ct)
        {
            var deleted = await _manufacturerService.Delete(manufacturerId, ct);

            if (!deleted)
                return NotFound();

            return NoContent();
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var allManufactures = await _manufacturerService.GetAll(ct);

            return Ok(allManufactures);
        }
    }
}
