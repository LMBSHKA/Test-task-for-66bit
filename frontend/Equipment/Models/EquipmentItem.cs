namespace Warehouse.Frontend.Equipment.Models;

internal sealed record EquipmentItem(
    Guid Id,
    string? Photo,
    string EquipmentTypeName,
    string ManufacturerName,
    string Model,
    string SupplyCountryName,
    decimal Price,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);
