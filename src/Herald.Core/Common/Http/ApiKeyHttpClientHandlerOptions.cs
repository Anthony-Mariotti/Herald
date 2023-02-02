namespace Herald.Core.Common.Http;

public class ApiKeyHttpClientHandlerOptions
{
    public ApiKeyMethod Method { get; set; } = ApiKeyMethod.Header;

    public string HeaderName { get; set; } = "X-API-KEY";

    public string QueryName { get; set; } = "apiKey";
}

public enum ApiKeyMethod
{
    Header,
    Query
}
