# Slo248.Net.DDD

A lightweight, production-ready Domain-Driven Design (DDD) library for .NET providing foundational abstractions and base classes for building domain models using DDD principles.

## Features

- **Aggregate Root Base Class** - Abstract base for implementing aggregate roots with domain event management
- **Entity Base Class** - Type-safe entity implementation with identity
- **Domain Event Support** - Built-in domain event publishing and handling
- **Repository Pattern** - Generic repository interface for data access abstraction
- **MediatR Integration** - Seamless integration with MediatR for event handling

## Installation

```bash
dotnet add package Slo248.Net.DDD
```

Or via Package Manager:
```
Install-Package Slo248.Net.DDD
```

## Quick Start

### Define an Entity

```csharp
using Slo248.Net.DDD.Models;

public class Product : Entity<ProductId>
{
    public string Name { get; set; }
    public decimal Price { get; set; }

    public Product(ProductId id, string name, decimal price)
        : base(id)
    {
        Name = name;
        Price = price;
    }
}

public record ProductId(Guid Value) : EntityId(Value);
```

### Define an Aggregate Root

```csharp
using Slo248.Net.DDD.Models;

public class Order : AggregateRoot<OrderId>
{
    private readonly List<OrderItem> _items = new();

    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();
    public OrderStatus Status { get; private set; }

    public Order(OrderId id) : base(id)
    {
        Status = OrderStatus.Pending;
    }

    public void AddItem(OrderItem item)
    {
        _items.Add(item);
        RaiseDomainEvent(new OrderItemAddedEvent(Id, item.ProductId, item.Quantity));
    }
}

public record OrderId(Guid Value) : EntityId(Value);
```

### Define a Domain Event

```csharp
using Slo248.Net.DDD.Models;

public record OrderItemAddedEvent(OrderId OrderId, ProductId ProductId, int Quantity)
    : IDomainEvent;
```

### Use with MediatR

```csharp
using MediatR;
using Slo248.Net.DDD.Services;

public class OrderEventHandler : IDomainEventPublisher
{
    private readonly IMediator _mediator;

    public OrderEventHandler(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        await _mediator.Publish((dynamic)domainEvent, cancellationToken);
    }
}
```

## Architecture

### Core Components

- **`Entity<TId>`** - Base class for domain entities with identity
- **`AggregateRoot<TId>`** - Base class for aggregate roots with domain event tracking
- **`EntityId`** - Abstract base class for strongly-typed entity identifiers
- **`IDomainEvent`** - Marker interface for domain events
- **`IRepository<TAggregateRoot, TId>`** - Generic repository interface
- **`IDomainEventPublisher`** - Domain event publication abstraction

## Requirements

- .NET 10.0+
- MediatR.Contracts 2.0+

## Documentation

For more information on Domain-Driven Design principles, visit:
- [Domain-Driven Design by Eric Evans](https://www.domainlanguage.com/ddd/)
- [Microsoft DDD Documentation](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice)

## License

MIT License - see LICENSE file for details

## Contributing

Contributions are welcome! Please feel free to submit a pull request.

## Support

If you encounter any issues or have questions, please open an issue on GitHub.

