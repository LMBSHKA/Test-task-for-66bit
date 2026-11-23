using System.ComponentModel.DataAnnotations;

namespace Application.DTO.EquipmentType
{
    public class UpdateEquipmentTypeDTO : IValidatableObject
    {
        public string? Name { get; set; } = null;
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
