using System.ComponentModel.DataAnnotations;

namespace Models.Entities
{
    public class EquipmentItem
    {
        [Key]
        public Guid Id { get; set; }
        public string? Photo { get; set; } = null;
        public Guid EquipmentTypeId { get; set; }
        public Guid ManufacturerId { get; set; }
        [Required]
        public string Model { get; set; }
        public Guid SupplyCountryId { get; set; }
        [Required]
        public decimal Price { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime DeletedAt { get; set; } = DateTime.MinValue;

        public EquipmentType EquipmentType { get; set; } = null!;
        public Manufacturer Manufacturer { get; set; } = null!;
        public Country SupplyCountry { get; set; } = null!;
    }
}
