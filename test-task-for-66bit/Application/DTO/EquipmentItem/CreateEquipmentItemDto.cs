using System.ComponentModel.DataAnnotations;

namespace Application.DTO.EquipmentItem
{
    public class CreateEquipmentItemDto
    {
        public string? Photo { get; set; } = null;
        public Guid EquipmentTypeId { get; set; }
        public Guid ManufacturerId { get; set; }
        [Required]
        public string Model { get; set; }
        public Guid SupplyCountryId { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
