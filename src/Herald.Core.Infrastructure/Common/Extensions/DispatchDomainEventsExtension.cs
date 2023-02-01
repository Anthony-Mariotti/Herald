using Herald.Core.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Herald.Core.Infrastructure.Common.Extensions;

public static partial class InfrastructureExtensions
{
    public static async Task DispatchDomainEvents(this IMediator mediator, DbContext context)
    {
        var entities = context.ChangeTracker
            .Entries<BaseDomainEntity>()
            .Where(e => e.Entity.DomainEvents.Any())
            .Select(e => e.Entity)
            .ToList();

        var domainEvents = entities
            .SelectMany(e => e.DomainEvents)
            .ToList();
        
        entities.ForEach(e => e.ClearDomainEvents());

        foreach (var @event in domainEvents)
        {
            await mediator.Publish(@event);
        }
    }
}