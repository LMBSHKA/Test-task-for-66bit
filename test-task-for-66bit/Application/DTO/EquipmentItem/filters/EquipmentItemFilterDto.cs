using System.ComponentModel.DataAnnotations;

namespace Application.DTO.EquipmentItem.filters
{
    public class EquipmentItemFilterDto : IValidatableObject
    {
        /// <summary>Поиск по части названия модели, типа техники или производителя без учёта регистра.</summary>
        public string? Search { get; set; }

        /// <summary>UUID типа техники для фильтрации.</summary>
        public Guid? EquipmentTypeId { get; set; }

        /// <summary>UUID производителя для фильтрации.</summary>
        public Guid? ManufacturerId { get; set; }

        /// <summary>Нижняя граница цены включительно.</summary>
        public decimal? MinPrice { get; set; }

        /// <summary>Верхняя граница цены включительно. Должна быть не меньше нижней, если указаны обе.</summary>
        public decimal? MaxPrice { get; set; }

        /// <summary>Номер страницы, начиная с 1. По умолчанию 1.</summary>
        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        /// <summary>Количество позиций на странице, от 1. По умолчанию 20.</summary>
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
