﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Herald.Core.Domain.Common;

public class BaseDomainEntity : BaseDomainEntity<long> { }

public class BaseDomainEntity<T> where T : struct
{
    public T Id { get; set; }

    private readonly List<BaseEvent> _domainEvents = new();

    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(BaseEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void RemoveDomainEvent(BaseEvent domainEvent) => _domainEvents.Remove(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}