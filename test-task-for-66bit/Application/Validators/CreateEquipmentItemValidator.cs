using Application.DTO.EquipmentItem;
using FluentValidation;

namespace Application.Validators
{
    public class CreateEquipmentItemValidator : AbstractValidator<CreateEquipmentItemDto>
    {
        public CreateEquipmentItemValidator()
        {
            RuleFor(x => x.Model).NotEmpty().MaximumLength(200);

            RuleFor(x => x.EquipmentTypeId).NotEmpty();
            RuleFor(x => x.ManufacturerId).NotEmpty();
            RuleFor(x => x.SupplyCountryId).NotEmpty();

            RuleFor(x => x.Price)
                .GreaterThan(0m)
                .WithMessage("Цена должна быть больше нуля.");
        }
    }
}
