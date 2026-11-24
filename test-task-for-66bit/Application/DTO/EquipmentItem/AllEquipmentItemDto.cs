using Application.DTO.EquipmentItem.filters;

namespace Application.DTO.EquipmentItem
{
    public class AllEquipmentItemDto
    {
        public List<EquipmentItemDto> EquipmentItems { get; set; } = new();
        public List<EquipmentItemFilterOptionDto> AvailableEquipmentTypes { get; set; } = new();
        public List<EquipmentItemFilterOptionDto> AvailableManufacturers { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
