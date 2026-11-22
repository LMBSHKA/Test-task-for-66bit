using System.ComponentModel.DataAnnotations;

namespace Models.Entities
{
    public class Country
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }

        public List<EquipmentItem> EquipmentItems { get; set; } = new List<EquipmentItem>();
    }
}
