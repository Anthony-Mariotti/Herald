using Herald.Core.Application.Abstractions;
using Herald.Core.Domain.Entities.AnyDeals;
using System.Net.Http.Json;

namespace Herald.Core.Infrastructure.Services;

public class AnyDealService : IAnyDealService
{
	private readonly HttpClient _client;

	public AnyDealService(HttpClient client)
	{
		_client = client;
	}

	public async Task<AnyDealDataList> SearchAsync(string name)
	{
		var response = await _client.GetFromJsonAsync<AnyDealData>($"v02/search/search/?q={name}&limit=5");

        return response is null
			? new AnyDealDataList()
			: response.Data;
    }
}
