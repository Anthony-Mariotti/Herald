namespace Herald.Core.Infrastructure.Common.Exceptions;

public class HeraldInfrastructureException : Exception
{
	public HeraldInfrastructureException()
	{

	}

	public HeraldInfrastructureException(string? message) : base(message)
	{

	}

	public HeraldInfrastructureException(string? message, Exception? exception) : base(message, exception)
	{

	}
}
