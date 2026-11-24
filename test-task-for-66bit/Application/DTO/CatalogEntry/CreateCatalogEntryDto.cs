using System.ComponentModel.DataAnnotations;

namespace Application.DTO.EquipmentType
{
    public class CreateCatalogEntryDto
    {
        /// <summary>Название записи справочника.</summary>
        [Required]
        public string Name { get; set; }

        /// <summary>Необязательное описание длиной до 2000 символов.</summary>
        [MaxLength(2000)]
        public string? Description { get; set; }
    }
}
