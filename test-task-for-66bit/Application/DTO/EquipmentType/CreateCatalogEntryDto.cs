using System.ComponentModel.DataAnnotations;

namespace Application.DTO.EquipmentType
{
    public class CreateCatalogEntryDto
    {
        [Required]
        public string Name { get; set; }
        [MaxLength(2000)]
        public string? Description { get; set; }
    }
}
