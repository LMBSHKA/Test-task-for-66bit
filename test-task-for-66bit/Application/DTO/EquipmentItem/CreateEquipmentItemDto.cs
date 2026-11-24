using System.ComponentModel.DataAnnotations;

namespace Application.DTO.EquipmentItem
{
    public class CreateEquipmentItemDto
    {
        /// <summary>Необязательный URL фотографии, полученный при её загрузке.</summary>
        public string? Photo { get; set; } = null;

        /// <summary>UUID существующего типа техники. Обязательное поле.</summary>
        public Guid EquipmentTypeId { get; set; }

        /// <summary>UUID существующего производителя. Обязательное поле.</summary>
        public Guid ManufacturerId { get; set; }

        /// <summary>Название модели длиной от 1 до 200 символов.</summary>
        [Required]
        public string Model { get; set; }

        /// <summary>UUID существующей страны поставки. Обязательное поле.</summary>
        public Guid SupplyCountryId { get; set; }

        /// <summary>Цена единицы оборудования. Должна быть больше нуля.</summary>
        [Required]
        public decimal Price { get; set; }
    }
}
