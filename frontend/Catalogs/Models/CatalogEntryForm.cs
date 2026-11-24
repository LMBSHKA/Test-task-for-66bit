using System.ComponentModel.DataAnnotations;

namespace Warehouse.Frontend.Catalogs.Models;

internal sealed class CatalogEntryForm
{
    [Required(ErrorMessage = "Введите наименование.")]
    public string Name { get; set; } = string.Empty;

    [StringLength(2000, ErrorMessage = "Описание должно содержать не более 2000 символов.")]
    public string Description { get; set; } = string.Empty;
}
