using MediatR;

namespace Herald.Core.Domain.Common;

public abstract class BaseEvent : INotification
{
    public string EventId { get; }

	public BaseEvent()
	{
		EventId = Guid.NewGuid().ToString("N").ToLower();
	}
}