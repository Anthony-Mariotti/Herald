using Herald.Core.Infrastructure.Common.Exceptions;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Herald.Core.Infrastructure.Common.Configuration;

public class DatabaseConfig
{
    private static string _cachedConnectionString = string.Empty;

    public string Server { get; set; } = default!;

    public int Port { get; set; } = 3306;

    public string Name { get; set; } = default!;

    public string Username { get; set; } = default!;

    public string Password { get; set; } = default!;

    public int ConnectionTimeout { get; set; } = 5;

    public int CommandTimeout { get; set; } = 10;

    public DatabaseConfigFeatures Feature { get; set; } = new();

    [SuppressMessage("Style", "IDE0058:Expression value is never used", Justification = "Readability")]
    public string ConnectionString
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(_cachedConnectionString))
            {
                return _cachedConnectionString;
            }

            var builder = new StringBuilder();
            if (IsValid)
            {
                builder.Append($"Server={Server};");

                builder.Append($"Port={Port};");
                builder.Append($"Database={Name};");
                builder.Append($"Uid={Username};");
                builder.Append($"Pwd={Password};");

                if (Feature.UsageAdvisor)
                {
                    builder.Append($"UseUsageAdvisor=True;");
                }

                if (Feature.PerformanceMonitor)
                {
                    builder.Append($"UsePerformanceMonitor=True;");
                }

                builder.Append($"Connection Timeout={ConnectionTimeout};");
                builder.Append($"default command timeout={CommandTimeout};");

                _cachedConnectionString = builder.ToString();
                return _cachedConnectionString;
            }

            throw new HeraldInfrastructureException("Failed to retreive connection string which is not valid");
        }
    }

    [SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "Readability")]
    public bool IsValid
    {
        get
        {
            if (string.IsNullOrWhiteSpace(Server))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(Name))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(Username))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                return false;
            }

            return true;
        }
    }
}

public class DatabaseConfigFeatures
{
    public bool UsageAdvisor { get; set; } = false;

    public bool PerformanceMonitor { get; set; } = false;
}
