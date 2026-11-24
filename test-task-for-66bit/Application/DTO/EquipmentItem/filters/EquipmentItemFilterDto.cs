using System.ComponentModel.DataAnnotations;

namespace Application.DTO.EquipmentItem.filters
{
    public class EquipmentItemFilterDto : IValidatableObject
    {
        public string? Search { get; set; }
        public Guid? EquipmentTypeId { get; set; }
        public Guid? ManufacturerId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;
        [Range(1, int.MaxValue)]
        public int PageSize { get; set; } = 20;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MinPrice.HasValue && MaxPrice.HasValue && MinPrice.Value > MaxPrice.Value)
            {
                yield return new ValidationResult(
                    "Максимальная цена должна быть не меньше минимальной.",
                    new[] { nameof(MaxPrice) });
            }
        }
    }
}
