using Herald.Core.Common.Http;
using System.Threading;

namespace Herald.Core.Application.Common.Http;

public class ApiKeyHttpClientHandler : HttpClientHandler
{
	private readonly ApiKeyHttpClientHandlerOptions _options;
	private readonly string _apiKey;

	public ApiKeyHttpClientHandler(ApiKeyHttpClientHandlerOptions options, string apiKey)
	{
		_options = options;
		_apiKey = apiKey;
	}

    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken)
	{
        ApplyApiKey(ref request);
        return base.Send(request, cancellationToken);
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		ApplyApiKey(ref request);
        return base.SendAsync(request, cancellationToken);
    }

	private void ApplyApiKey(ref HttpRequestMessage request)
	{
        if (_options.Method == ApiKeyMethod.Query)
        {
            if (string.IsNullOrWhiteSpace(_options.QueryName))
            {
                if (!request.RequestUri?.Query.Contains("apiKey") ?? false)
                {
                    request.RequestUri = new Uri(
                        request.RequestUri?.AbsoluteUri + (request.RequestUri?.Query == "" ? "?" : "&") + $"apiKey=" + _apiKey
                    );
                }
            }
            else
            {
                if (!request.RequestUri?.Query.Contains(_options.QueryName) ?? false)
                {
                    request.RequestUri = new Uri(
                        request.RequestUri?.AbsoluteUri + (request.RequestUri?.Query == "" ? "?" : "&") + $"{_options.QueryName}=" + _apiKey
                    );
                }
            }

            return;
        }

        if (string.IsNullOrWhiteSpace(_options.HeaderName))
        {
            if (!request.Headers.Contains("X-API-KEY"))
            {
                request.Headers.Add("X-API-KEY", _apiKey);
            }
        }
        else
        {
            if (!request.Headers.Contains(_options.HeaderName))
            {
                request.Headers.Add(_options.HeaderName, _apiKey);
            }
        }
    }
}
