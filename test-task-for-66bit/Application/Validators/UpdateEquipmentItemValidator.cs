using Application.DTO.EquipmentItem;
using FluentValidation;

namespace Application.Validators
{
    public class UpdateEquipmentItemValidator : AbstractValidator<UpdateEquipmentItemDto>
    {
        public UpdateEquipmentItemValidator()
        {
            RuleFor(x => x.Model).NotEmpty().MaximumLength(200).When(x => x.Model != null);

            RuleFor(x => x.EquipmentTypeId).NotEqual(Guid.Empty);
            RuleFor(x => x.ManufacturerId).NotEqual(Guid.Empty);
            RuleFor(x => x.SupplyCountryId).NotEqual(Guid.Empty);

            RuleFor(x => x.Price)
                .GreaterThan(0m)
                .WithMessage("Цена должна быть больше нуля.");
        }
    }
}
