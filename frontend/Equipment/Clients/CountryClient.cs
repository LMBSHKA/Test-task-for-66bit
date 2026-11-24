using System.Net.Http.Json;
using System.Text.Json;
using Warehouse.Frontend.Equipment.Models;
using Warehouse.Frontend.Shared;

namespace Warehouse.Frontend.Equipment.Clients;

internal sealed class CountryClient(HttpClient http)
{
    public async Task<IReadOnlyList<CountryOption>> GetOptionsAsync(CancellationToken cancellationToken)
    {
        var response = await http.GetFromJsonAsync<CountryList>("api/country/all", cancellationToken);
        return response?.ListCountry ?? throw new JsonException("The country response did not contain a list.");
    }

    public async Task<CountryOption> AddAsync(string name, CancellationToken cancellationToken)
    {
        using var response = await http.PostAsJsonAsync("api/country/create", new { Name = name }, cancellationToken);
        await ApiRequestException.EnsureSuccessAsync(response, "Не удалось добавить страну.", cancellationToken);
        var id = await response.Content.ReadFromJsonAsync<Guid>(cancellationToken);
        if (id == Guid.Empty)
            throw new JsonException("The country response did not contain an ID.");

        return new CountryOption(id, name.ToUpperInvariant());
    }

    private sealed record CountryList(List<CountryOption> ListCountry);
}
