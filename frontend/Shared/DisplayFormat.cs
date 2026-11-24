using System.Globalization;

namespace Warehouse.Frontend.Shared;

internal static class DisplayFormat
{
    public static string Date(DateTimeOffset? value)
        => value is { } date && date != default
            ? date.ToLocalTime().ToString("dd.MM.yyyy, HH:mm", CultureInfo.InvariantCulture)
            : "—";
}
