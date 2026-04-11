# Slo248.Net.Basis

A comprehensive .NET solution providing foundational libraries and utilities for building modern, maintainable applications following Domain-Driven Design (DDD) principles and best practices.

## Overview

Slo248.Net.Basis is a collection of NuGet packages designed to accelerate .NET development with proven architectural patterns, abstractions, and utilities. It focuses on providing clean, reusable, and production-ready components for enterprise applications.

## Projects

### 🏗️ Slo248.Net.DomainDrivenDesign

A lightweight, production-ready Domain-Driven Design library providing foundational abstractions and base classes for building robust domain models.

**Key Features:**
- Aggregate Root and Entity base classes
- Domain event management and publishing
- Repository pattern abstractions
- MediatR integration for event handling
- Strongly-typed entity identifiers

**Installation:**
```bash
dotnet add package Slo248.Net.DomainDrivenDesign
```

[Learn more →](./Slo248.Net.DomainDrivenDesign/README.md)

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
using Slo248.Net.DomainDrivenDesign.Models;

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

### Publishing Domain Events with MediatR

```csharp
using MediatR;
using Slo248.Net.DomainDrivenDesign.Services;

public class OrderEventPublisher : IDomainEventPublisher
{
    private readonly IMediator _mediator;

    public OrderEventPublisher(IMediator mediator) => _mediator = mediator;

    public async Task PublishAsync(IDomainEvent domainEvent, CancellationToken cancellationToken = default)
    {
        await _mediator.Publish((dynamic)domainEvent, cancellationToken);
    }
}
```

## Requirements

- **.NET 10.0** or later
- **MediatR.Contracts 2.0** or later (for DDD package)

## Architecture & Design

The Slo248.Net.Basis solution follows these architectural principles:

- **Domain-Driven Design** - Clean domain models with proper separation of concerns
- **SOLID Principles** - Maintainable, testable, and extensible code
- **Repository Pattern** - Data access abstraction and testability
- **Event-Driven Architecture** - Decoupled, scalable communication patterns
- **Strongly-Typed Identifiers** - Type safety for aggregate root and entity IDs

## Solution Structure

```
Slo248.Net.Basis/
├── Slo248.Net.DomainDrivenDesign/    # Core DDD library (NuGet package)
│   ├── Models/                       # Base classes and interfaces
│   └── Services/                     # Repository and event abstractions
├── Slo248.Net.Tests.Core/            # Testing utilities and fixtures
└── Slo248.Net.Tests.UnitTests/       # Comprehensive test suite
```

## NuGet Packages

| Package | Description | Status |
|---------|-------------|--------|
| **Slo248.Net.DomainDrivenDesign** | DDD patterns and abstractions | ![NuGet](https://img.shields.io/nuget/v/Slo248.Net.DomainDrivenDesign) |

## Getting Started

1. **Install the NuGet package:**
   ```bash
   dotnet add package Slo248.Net.DomainDrivenDesign
   ```

2. **Define your domain models:**
   - Create strongly-typed IDs inheriting from `EntityId`
   - Implement entities inheriting from `Entity<TId>`
   - Implement aggregate roots inheriting from `AggregateRoot<TId>`

3. **Define domain events:**
   - Create records implementing `IDomainEvent`
   - Raise them using `RaiseDomainEvent()` in your aggregates

4. **Handle events:**
   - Implement MediatR notification handlers for your events
   - Use `IDomainEventPublisher` for event distribution

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

