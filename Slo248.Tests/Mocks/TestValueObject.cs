// Copyright (c) Slo248.
// Licensed under the MIT License.

using Slo248.Net.DomainDrivenDesign.Models;

namespace Slo248.Tests.Mocks;

/// <summary>
/// A test implementation of ValueObject for testing purposes.
/// Represents a money value object with amount and currency.
/// </summary>
public class TestMoney : ValueObject
{
    public decimal Amount { get; }
    public string Currency { get; }

    public TestMoney(decimal amount, string currency)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Amount cannot be negative", nameof(amount));
        }

        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new ArgumentException("Currency cannot be null or empty", nameof(currency));
        }

        Amount = amount;
        Currency = currency;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Amount;
        yield return Currency;
    }
}

/// <summary>
/// Another test implementation of ValueObject for testing type checking.
/// Represents an address value object.
/// </summary>
public class TestAddress : ValueObject
{
    public string Street { get; }
    public string City { get; }
    public string PostalCode { get; }

    public TestAddress(string street, string city, string postalCode)
    {
        if (string.IsNullOrWhiteSpace(street))
        {
            throw new ArgumentException("Street cannot be null or empty", nameof(street));
        }

        if (string.IsNullOrWhiteSpace(city))
        {
            throw new ArgumentException("City cannot be null or empty", nameof(city));
        }

        if (string.IsNullOrWhiteSpace(postalCode))
        {
            throw new ArgumentException("PostalCode cannot be null or empty", nameof(postalCode));
        }

        Street = street;
        City = city;
        PostalCode = postalCode;
    }

    protected override IEnumerable<object?> GetAtomicValues()
    {
        yield return Street;
        yield return City;
        yield return PostalCode;
    }
}
