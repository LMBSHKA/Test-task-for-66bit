using System.ComponentModel.DataAnnotations;

namespace Application.DTO.EquipmentType
{
    public class GetEquipmentTypeDTO
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [MaxLength(2000)]
        public string? Description { get; set; }
    }
}
