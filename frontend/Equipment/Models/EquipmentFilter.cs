namespace Warehouse.Frontend.Equipment.Models;

internal sealed record EquipmentFilter(
    string Search,
    Guid? EquipmentTypeId,
    Guid? ManufacturerId,
    decimal? MinPrice,
    decimal? MaxPrice,
    int Page);
