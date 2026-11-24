namespace Warehouse.Frontend.Equipment.Models;

public sealed record EquipmentItem(
    Guid Id,
    string? Photo,
    string EquipmentTypeName,
    Guid EquipmentTypeId,
    string ManufacturerName,
    Guid ManufacturerId,
    string Model,
    string SupplyCountryName,
    Guid SupplyCountryId,
    decimal Price,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
