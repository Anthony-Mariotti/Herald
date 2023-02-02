using Herald.Core.Application.Abstractions;
using Herald.Core.Application.Common.Http;
using Herald.Core.Common.Http;
using Herald.Core.Configuration;
using Herald.Core.Infrastructure.Common.Exceptions;
using Herald.Core.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System.Net;

namespace Herald.Core.Infrastructure.Common.Extensions;

public static partial class InfrastructureExtensions
{
    public static IServiceCollection AddAnyDealService(this IServiceCollection services, IConfiguration configuration)
    {
        var apiKey = configuration.GetSection(nameof(HeraldConfig)).GetValue<string>("AnyDealApiKey");

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new HeraldInfrastructureException("Invalid isthereanydeal api key");
        }

        services.AddHttpClient<IAnyDealService, AnyDealService>(client =>
        {
            client.BaseAddress = new Uri("https://api.isthereanydeal.com/");
            client.Timeout = TimeSpan.FromSeconds(3);
        })
            .ConfigurePrimaryHttpMessageHandler(() => new ApiKeyHttpClientHandler(new ApiKeyHttpClientHandlerOptions
            {
                Method = ApiKeyMethod.Query,
                QueryName = "key"
            }, apiKey))
            .AddPolicyHandler(GetRetryPolicy());

        return services;
    }

    private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
            .WaitAndRetryAsync(6, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)));
    }
}
