# Slo248.Net.Basis

A comprehensive .NET solution providing foundational libraries and utilities for building modern, maintainable applications following Domain-Driven Design (DDD) principles and best practices.

---

[![NuGet](https://img.shields.io/nuget/v/Slo248.Net.DDD.svg)](https://www.nuget.org/packages/Slo248.Net.DDD)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](./LICENSE)

---

## Overview

Slo248.Net.Basis is a collection of NuGet packages designed to accelerate .NET development with proven architectural patterns, abstractions, and utilities. It focuses on providing clean, reusable, and production-ready components for enterprise applications.

## Projects

### 🏗️ Slo248.Net.DDD

A lightweight, production-ready Domain-Driven Design library for .NET 10.0+ providing foundational abstractions and base classes for building robust domain models using DDD principles.

**Key Features:**
- Generic, type-safe Aggregate Root and Entity base classes with dual generic parameters
- Value Objects with atomic value composition and value-based equality
- Domain event management and event sourcing support
- Repository pattern abstractions
- MediatR integration for event handling
- Strongly-typed, reusable entity identifiers with implicit conversions

**Installation:**
```bash
dotnet add package Slo248.Net.DDD
```

[Learn more →](./Slo248.Net.DDD/README.md)

### 🧪 Slo248.Net.Tests.Core

Core testing utilities and base classes for unit testing within the Slo248.Net ecosystem. Provides common test fixtures, domain event test helpers, and entity test utilities.

### ✅ Slo248.Net.Tests.UnitTests

Comprehensive unit test suite demonstrating and validating the functionality of core libraries. Includes tests for:
- Aggregate root behavior and domain events
- Entity identity and equality
- Repository patterns
- Event publishing mechanisms

## Quick Start

### Creating an Aggregate Root

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

    public Order() : base()  // Auto-generates Guid
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

### Publishing Domain Events with MediatR

```csharp
using MediatR;
using Slo248.Net.DDD.Models;
using Slo248.Net.DDD.Services;

public record OrderItemAddedEvent(Guid OrderId, Guid ProductId, int Quantity) : IDomainEvent;

public class DomainEventPublisher : IDomainEventPublisher
{
    private readonly IMediator _mediator;

    public DomainEventPublisher(IMediator mediator) => _mediator = mediator;

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

## Requirements

- **.NET 10.0** or later
- **(Optional) MediatR 12.0** or later – for event handling integration

## Architecture & Design

The Slo248.Net.Basis solution follows these architectural principles:

- **Domain-Driven Design** – Clean, cohesive domain models with proper separation of concerns
- **SOLID Principles** – Maintainable, testable, and extensible code
- **Repository Pattern** – Data access abstraction with event sourcing support
- **Event-Driven Architecture** – Decoupled, scalable communication patterns via domain events
- **Strongly-Typed Identifiers** – Generic, reusable `EntityId<TId, TEntity>` for type-safe ID management
- **Value Objects** – Immutable objects with value-based equality and atomic composition
- **Event Sourcing** – Track and manage uncommitted events for eventual consistency patterns

## Solution Structure

```
Slo248.Net.Basis/
├── Slo248.Net.DDD/                   # Core DDD library (NuGet package)
│   ├── Models/                       # Base classes and interfaces
│   └── Services/                     # Repository and event abstractions
├── Slo248.Net.Tests.Core/            # Testing utilities and fixtures
└── Slo248.Net.Tests.UnitTests/       # Comprehensive test suite
```

## NuGet Packages

| Package | Description | Status |
|---------|-------------|--------|
| **Slo248.Net.DDD** | DDD patterns and abstractions | ![NuGet](https://img.shields.io/nuget/v/Slo248.Net.DDD) |

## Getting Started

1. **Install the NuGet package:**
   ```bash
   dotnet add package Slo248.Net.DDD
   ```

2. **Define your domain models:**
   - Create aggregate roots inheriting from `AggregateRoot<TId, TEntity>` or `AggregateRoot<TEntity>` (for Guid)
   - Create child entities inheriting from `Entity<TId, TEntity>` or `Entity<TEntity>`
   - Use `EntityId<TId, TEntity>` for strongly-typed identifiers, or `EntityId<TEntity>` for Guid-based IDs

3. **Define domain events:**
   - Create records implementing `IDomainEvent`
   - Raise them using `RaiseDomainEvent()` in your aggregates
   - Track uncommitted events with `GetUncommittedEvents()`

4. **Handle events after persistence:**
   - Implement `IDomainEventPublisher` or use the MediatR integration
   - Call `MarkAsCommitted()` on the aggregate after event publication

5. **Implement repositories:**
   - Create repository classes implementing `IRepository<TAggregateRoot, TId>`
   - Publish events before or after persistence based on your consistency requirements

## Contributing

Contributions are welcome! To contribute:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## Resources

- [Domain-Driven Design by Eric Evans](https://www.domainlanguage.com/ddd/)
- [Microsoft DDD Documentation](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/ddd-oriented-microservice)
- [NuGet Best Practices](https://aka.ms/nuget/authoring-best-practices/readme)

## License

MIT License - see [LICENSE](./LICENSE) file for details

## Support

For issues, questions, or suggestions:
- 📝 Open an issue on GitHub
- 💬 Start a discussion
- 📧 Contact the maintainers

---

**Built with ❤️ for the .NET community**
