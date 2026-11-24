using System.ComponentModel.DataAnnotations;

namespace Warehouse.Frontend.Equipment.Models;

internal sealed class EquipmentItemForm
{
    [Required(ErrorMessage = "Введите модель.")]
    [StringLength(200, ErrorMessage = "Модель должна содержать не более 200 символов.")]
    public string Model { get; set; } = string.Empty;

    [Required(ErrorMessage = "Выберите тип техники.")]
    public Guid? EquipmentTypeId { get; set; }

    [Required(ErrorMessage = "Выберите производителя.")]
    public Guid? ManufacturerId { get; set; }

    [Required(ErrorMessage = "Выберите страну поставки.")]
    public Guid? SupplyCountryId { get; set; }

    [Required(ErrorMessage = "Введите цену.")]
    [Range(typeof(decimal), "0", "79228162514264337593543950335", MinimumIsExclusive = true,
        ErrorMessage = "Введите положительную цену.")]
    public decimal? Price { get; set; }
}
