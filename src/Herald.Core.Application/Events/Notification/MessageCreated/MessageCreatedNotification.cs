using DSharpPlus.EventArgs;
using MediatR;

namespace Herald.Core.Application.Events.Notification.MessageCreated;

public record MessageCreatedNotification(MessageCreateEventArgs EventArgs) : INotification;