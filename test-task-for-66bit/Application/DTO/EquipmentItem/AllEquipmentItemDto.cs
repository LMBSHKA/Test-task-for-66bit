using Application.DTO.EquipmentItem.filters;

namespace Application.DTO.EquipmentItem
{
    public class AllEquipmentItemDto
    {
        public List<EquipmentItemDto> EquipmentItems { get; set; } = new();

        /// <summary>Варианты фильтра по типу техники с учётом выбранного производителя и позиций на всех страницах.</summary>
        public List<EquipmentItemFilterOptionDto> AvailableEquipmentTypes { get; set; } = new();

        /// <summary>Варианты фильтра по производителю с учётом выбранного типа техники и позиций на всех страницах.</summary>
        public List<EquipmentItemFilterOptionDto> AvailableManufacturers { get; set; } = new();

        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
