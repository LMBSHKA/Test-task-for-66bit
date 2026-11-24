namespace Application.DTO.EquipmentItem
{
    public class EquipmentItemDto
    {
        public Guid Id { get; set; }
        public string? Photo { get; set; }
        public string EquipmentTypeName { get; set; }
        public Guid EquipmentTypeId { get; set; }
        public string ManufacturerName { get; set; }
        public Guid ManufacturerId { get; set; }
        public string Model { get; set; }
        public string SupplyCountryName { get; set; }
        public Guid SupplyCountryId { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
