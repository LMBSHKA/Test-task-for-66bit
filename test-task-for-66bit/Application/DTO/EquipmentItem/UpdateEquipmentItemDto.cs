namespace Application.DTO.EquipmentItem
{
    public class UpdateEquipmentItemDto
    {
        private string? _photo;

        /// <summary>Новый URL фотографии. Пропустите поле, чтобы сохранить фото, или передайте <c>null</c>, чтобы его отвязать.</summary>
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

        /// <summary>UUID нового типа техники из справочника.</summary>
        public Guid? EquipmentTypeId { get; set; }

        /// <summary>UUID нового производителя из справочника.</summary>
        public Guid? ManufacturerId { get; set; }

        /// <summary>Новое название модели длиной от 1 до 200 символов.</summary>
        public string? Model { get; set; }

        /// <summary>UUID новой страны поставки из справочника.</summary>
        public Guid? SupplyCountryId { get; set; }

        /// <summary>Новая цена единицы оборудования. Должна быть больше нуля.</summary>
        public decimal? Price { get; set; }
    }
}
