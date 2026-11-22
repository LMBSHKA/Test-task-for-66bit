using System.ComponentModel.DataAnnotations;

namespace Models.Entities
{
    public class Manufacturer
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; } = null;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public List<EquipmentItem> EquipmentItems { get; set; } = new List<EquipmentItem>();
    }
}
