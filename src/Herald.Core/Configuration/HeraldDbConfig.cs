// ReSharper disable MemberCanBePrivate.Global
namespace Herald.Core.Configuration;

public class HeraldDbConfig
{
    public string Host { get; set; } = "localhost";

    public int Port { get; set; } = 27017;

    public string ConnectionString => $"mongodb://{Host}:{Port}";
}