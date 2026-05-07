# Slo248.Net.DDD

A lightweight, production-ready Domain-Driven Design (DDD) library for .NET 10.0+ providing foundational abstractions and base classes for building domain models using DDD principles. Features strongly-typed entities, aggregate roots, value objects, and domain event management.

---

[![NuGet](https://img.shields.io/nuget/v/Slo248.Net.DDD.svg)](https://www.nuget.org/packages/Slo248.Net.DDD)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

---

## Features

- **Aggregate Root Base Class** – Abstract base class for implementing aggregate roots with built-in domain event tracking and management
- **Entity Base Class** – Generic, type-safe entity implementation with strong identity types
- **Value Object Base Class** – Immutable value objects with value-based equality and atomic value composition
- **Domain Event Support** – Flexible domain event publishing and tracking within aggregates
- **Type-Safe EntityId** – Generic, reusable strongly-typed entity identifiers with implicit conversions
- **Repository Pattern** – Generic repository interface for data access abstraction
- **Event Sourcing Ready** – Track uncommitted events with `GetUncommittedEvents()` and `MarkAsCommitted()` methods

## Installation

Install via NuGet Package Manager:

```bash
dotnet add package Slo248.Net.DDD
```

Or via Package Manager Console:

```
Install-Package Slo248.Net.DDD
```

## Quick Start

### 1. Define Entity Identifiers

Create strongly-typed entity identifiers by using the generic `EntityId<TId, TEntity>` class:

```csharp
using Slo248.Net.DDD.Models;

public class Product : BaseEntity<Guid, Product>
{
    public string Name { get; set; }
    public decimal Price { get; set; }

    public Product(EntityId<Guid, Product> id, string name, decimal price)
        : base(id)
    {
        Name = name;
        Price = price;
    }

    protected override void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        // Handle domain events in the entity
    }
}
```

### 2. Define an Aggregate Root

Create an aggregate root to manage domain entities and domain events:

```csharp
using Slo248.Net.DDD.Models;

public class Order : AggregateRoot<Guid, Order>
{
    private readonly List<OrderItem> _items = [];

    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();
    public OrderStatus Status { get; private set; }

    public Order(EntityId<Guid, Order> id) : base(id)
    {
        Status = OrderStatus.Pending;
    }

    public Order() : base()  // Generates a new Guid automatically
    {
        Status = OrderStatus.Pending;
    }

    public void AddItem(OrderItem item)
    {
        _items.Add(item);
        RaiseDomainEvent(new OrderItemAddedEvent(Id.Value, item.ProductId, item.Quantity));
    }
}

public enum OrderStatus { Pending, Completed, Cancelled }
```

### 3. Define a Domain Event

```csharp
using Slo248.Net.DDD.Models;

public record OrderItemAddedEvent(Guid OrderId, Guid ProductId, int Quantity) : IDomainEvent;
```

### 4. Define a Value Object

```csharp
using Slo248.Net.DDD.Models;

public class Money : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Amount;
        yield return Currency;
    }
}
```

### 5. Create a Repository Implementation

```csharp
using Slo248.Net.DDD.Models;
using Slo248.Net.DDD.Services;

public class OrderRepository : IRepository<Order, Guid>
{
    private readonly IDbContext _dbContext;
    private readonly IDomainEventPublisher _eventPublisher;

    public OrderRepository(IDbContext dbContext, IDomainEventPublisher eventPublisher)
    {
        _dbContext = dbContext;
        _eventPublisher = eventPublisher;
    }

    public async Task<Order?> Get(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Orders.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task Add(Order aggregate, CancellationToken cancellationToken = default)
    {
        await _dbContext.Orders.AddAsync(aggregate, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        // Publish domain events
        var events = aggregate.GetUncommittedEvents().ToArray();
        if (events.Length > 0)
        {
            await _eventPublisher.Publish(events, cancellationToken);
            aggregate.MarkAsCommitted();
        }
    }

    public async Task Update(Order aggregateRoot, CancellationToken cancellationToken = default)
    {
        _dbContext.Orders.Update(aggregateRoot);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var events = aggregateRoot.GetUncommittedEvents().ToArray();
        if (events.Length > 0)
        {
            await _eventPublisher.Publish(events, cancellationToken);
            aggregateRoot.MarkAsCommitted();
        }
    }

    public async Task Delete(Order aggregateRoot, CancellationToken cancellationToken = default)
    {
        _dbContext.Orders.Remove(aggregateRoot);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
```

### 6. Publish Domain Events

```csharp
using MediatR;
using Slo248.Net.DDD.Services;

public class DomainEventPublisher : IDomainEventPublisher
{
    private readonly IMediator _mediator;

    public DomainEventPublisher(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Publish(IDomainEvent[] events, CancellationToken cancellationToken = default)
    {
        foreach (var domainEvent in events)
        {
            await _mediator.Publish((dynamic)domainEvent, cancellationToken);
        }
    }
}

public class OrderItemAddedEventHandler : INotificationHandler<OrderItemAddedEvent>
{
    public async Task Handle(OrderItemAddedEvent notification, CancellationToken cancellationToken)
    {
        // Handle the order item added event
        await Task.CompletedTask;
    }
}
```

## Architecture

### Core Components

- **`BaseEntity<TId, TEntity>`** – Abstract base class for all domain entities with identity and domain event support
- **`AggregateRoot<TId, TEntity>`** – Extends `BaseEntity<TId, TEntity>` for aggregate root implementation with domain event tracking
- **`AggregateRoot<TEntity>`** – Convenience variant using `Guid` as the identity type
- **`Entity<TId, TEntity>`** – Extends `BaseEntity<TId, TEntity>` for use within aggregates, with event propagation to the root
- **`Entity<TEntity>`** – Convenience variant using `Guid` as the identity type
- **`EntityId<TId, TEntity>`** – Strongly-typed generic identifier for entities with value-based equality
- **`EntityId<TEntity>`** – Convenience variant for `Guid`-based identities
- **`ValueObject`** – Base class for immutable value objects with atomic value composition
- **`ValueObject<T>`** – Generic convenience variant for simple single-value value objects
- **`IDomainEvent`** – Marker interface for domain events
- **`IAggregateRoot`** – Interface for aggregate root contracts
- **`IRepository<TAggregateRoot, TId>`** – Generic repository interface for data access abstraction
- **`IDomainEventPublisher`** – Domain event publication abstraction for handling committed events

## Requirements

- **.NET 10.0** or later
- **(Optional) MediatR 12.0** or later – for event handling integration (recommended but not required)

## Package Metadata

- **Package ID:** `Slo248.Net.DDD`
- **Latest Version:** 1.0.1
- **License:** MIT
- **Repository:** https://github.com/slo248/Net.Basis

## Key Design Patterns

### Event Sourcing
Aggregate roots track all domain events that occur within their boundary via `GetUncommittedEvents()`. After persisting the aggregate, call `MarkAsCommitted()` to clear the event list.

### Value Objects
Use `ValueObject` for immutable objects that represent a concept and whose equality is based on atomic values, not identity.

### Strong Typing
The generic `EntityId<TId, TEntity>` enables compile-time type safety. Entities can only reference other entities through their proper ID types.

### Event-Driven Architecture
Domain events enable decoupling between aggregates. Use `IDomainEventPublisher` to handle events after aggregate persistence.

## Usage Tips

1. **Keep Aggregates Small** – Only include entities that must be modified together within a single transaction
2. **Use Value Objects** – Encapsulate related values (e.g., Money, Address) as value objects for domain clarity
3. **Domain Events** – Raise domain events for important state changes within aggregate roots
4. **Repository Pattern** – Access aggregates only through repositories, never directly modify via ORM
5. **Event Publishing** – Publish events after successful persistence to ensure consistency

## Documentation

For more information on Domain-Driven Design principles, see:

- [Domain-Driven Design by Eric Evans](https://www.domainlanguage.com/ddd/)
- [Microsoft DDD Documentation](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice)
- [Event Sourcing Pattern](https://martinfowler.com/eaaDev/EventSourcing.html)

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Contributing

Contributions are welcome! Please feel free to submit a pull request or open an issue.

## Support

If you encounter any issues or have questions, please open an issue on GitHub.
