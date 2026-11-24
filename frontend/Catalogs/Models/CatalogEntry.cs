namespace Warehouse.Frontend.Catalogs.Models;

public sealed record CatalogEntry(
    Guid Id,
    string Name,
    string? Description,
    DateTimeOffset? CreatedAt,
    DateTimeOffset? UpdatedAt);
