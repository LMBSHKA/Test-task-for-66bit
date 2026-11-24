namespace Warehouse.Frontend.Catalogs;

public sealed record CatalogEntry(
    Guid Id,
    string Name,
    string? Description,
    DateTimeOffset? CreatedAt,
    DateTimeOffset? UpdatedAt);
