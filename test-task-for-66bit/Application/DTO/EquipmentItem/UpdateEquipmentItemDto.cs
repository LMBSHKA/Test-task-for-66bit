namespace Application.DTO.EquipmentItem
{
    public class UpdateEquipmentItemDto
    {
        private string? _photo;

        public string? Photo
        {
            get => _photo;
            set
            {
                _photo = value;
                HasPhoto = true;
            }
        }

        internal bool HasPhoto { get; private set; }
        public Guid? EquipmentTypeId { get; set; }
        public Guid? ManufacturerId { get; set; }
        public string? Model { get; set; }
        public Guid? SupplyCountryId { get; set; }
        public decimal? Price { get; set; }
    }
}
