using System.ComponentModel.DataAnnotations;

namespace Application.DTO.EquipmentType
{
    public class UpdateCatalogEntryDto : IValidatableObject
    {
        /// <summary>Новое название записи. Если указано, должно содержать хотя бы один непробельный символ.</summary>
        public string? Name { get; set; } = null;

        /// <summary>Новое описание длиной до 2000 символов. Пустая строка очищает описание.</summary>
        [MaxLength(2000)]
        public string? Description { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Name is not null && string.IsNullOrWhiteSpace(Name))
            {
                yield return new ValidationResult(
                    "Название не может быть пустым или состоять только из пробелов.",
                    new[] { nameof(Name) });
            }
        }
    }
}
