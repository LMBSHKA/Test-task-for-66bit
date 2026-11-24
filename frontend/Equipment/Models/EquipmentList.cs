namespace Warehouse.Frontend.Equipment.Models;

internal sealed record EquipmentList(
    List<EquipmentItem> EquipmentItems,
    List<EquipmentFilterOption> AvailableEquipmentTypes,
    List<EquipmentFilterOption> AvailableManufacturers,
    int TotalCount,
    int Page,
    int PageSize)
{
    public int PageCount => TotalCount == 0 ? 1 : (TotalCount - 1) / PageSize + 1;
}
