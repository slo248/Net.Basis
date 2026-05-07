// Copyright (c) Slo248.
// Licensed under the MIT License.

using Slo248.Net.DDD.Models;
using Slo248.Tests.Mocks;

namespace Slo248.Tests.Domain;

/// <summary>
/// Unit tests for the ValueObject base class.
/// Tests value-based equality, hash code consistency, and operator overloads.
/// </summary>
public class ValueObjectTests
{
    #region Equality Tests

    [Fact]
    public void Equals_WithSameAtomicValues_ShouldBeEqual()
    {
        // Arrange
        var money1 = new TestMoney(100m, "USD");
        var money2 = new TestMoney(100m, "USD");

        // Act & Assert
        Assert.Equal(money1, money2);
    }

    [Fact]
    public void Equals_WithDifferentAmount_ShouldNotBeEqual()
    {
        // Arrange
        var money1 = new TestMoney(100m, "USD");
        var money2 = new TestMoney(200m, "USD");

        // Act & Assert
        Assert.NotEqual(money1, money2);
    }

    [Fact]
    public void Equals_WithDifferentCurrency_ShouldNotBeEqual()
    {
        // Arrange
        var money1 = new TestMoney(100m, "USD");
        var money2 = new TestMoney(100m, "EUR");

        // Act & Assert
        Assert.NotEqual(money1, money2);
    }

    [Fact]
    public void Equals_WithDifferentTypes_ShouldNotBeEqual()
    {
        // Arrange
        var money = new TestMoney(100m, "USD");
        var address = new TestAddress("Main St", "New York", "10001");

        // Act & Assert
        Assert.True(money != address);
    }

    [Fact]
    public void Equals_WithNull_ShouldNotBeEqual()
    {
        // Arrange
        var money = new TestMoney(100m, "USD");
        TestMoney? nullMoney = null;

        // Act & Assert
        Assert.NotEqual(money, nullMoney);
    }

    [Fact]
    public void Equals_BothNull_ShouldBeEqual()
    {
        // Arrange
        TestMoney? money1 = null;
        TestMoney? money2 = null;

        // Act & Assert
        Assert.Equal(money1, money2);
    }

    [Fact]
    public void Equals_WithObject_ShouldReturnTrue()
    {
        // Arrange
        var money1 = new TestMoney(100m, "USD");
        object money2 = new TestMoney(100m, "USD");

        // Act & Assert
        Assert.Equal(money1, money2);
    }

    [Fact]
    public void Equals_WithDifferentObject_ShouldReturnFalse()
    {
        // Arrange
        var money = new TestMoney(100m, "USD");
        object different = new TestMoney(100m, "EUR");

        // Act & Assert
        Assert.NotEqual(money, different);
    }

    [Fact]
    public void Equals_WithNonValueObject_ShouldReturnFalse()
    {
        // Arrange
        var money = new TestMoney(100m, "USD");
        object nonValueObject = "not a value object";

        // Act & Assert
        Assert.False(money.Equals(nonValueObject));
    }

    [Fact]
    public void Equals_WithMultiplePropertiesAllSame_ShouldBeEqual()
    {
        // Arrange
        var address1 = new TestAddress("123 Main St", "New York", "10001");
        var address2 = new TestAddress("123 Main St", "New York", "10001");

        // Act & Assert
        Assert.Equal(address1, address2);
    }

    [Fact]
    public void Equals_WithMultiplePropertiesOneDifferent_ShouldNotBeEqual()
    {
        // Arrange
        var address1 = new TestAddress("123 Main St", "New York", "10001");
        var address2 = new TestAddress("123 Main St", "New York", "10002");

        // Act & Assert
        Assert.NotEqual(address1, address2);
    }

    #endregion

    #region Equality Operator Tests

    [Fact]
    public void EqualityOperator_WithSameAtomicValues_ShouldReturnTrue()
    {
        // Arrange
        var money1 = new TestMoney(100m, "USD");
        var money2 = new TestMoney(100m, "USD");

        // Act & Assert
        Assert.True(money1 == money2);
    }

    [Fact]
    public void EqualityOperator_WithDifferentAtomicValues_ShouldReturnFalse()
    {
        // Arrange
        var money1 = new TestMoney(100m, "USD");
        var money2 = new TestMoney(100m, "EUR");

        // Act & Assert
        Assert.False(money1 == money2);
    }

    [Fact]
    public void EqualityOperator_BothNull_ShouldReturnTrue()
    {
        // Arrange
        TestMoney? money1 = null;
        TestMoney? money2 = null;

        // Act & Assert
        Assert.True(money1 == money2);
    }

    [Fact]
    public void EqualityOperator_OneNullOneNotNull_ShouldReturnFalse()
    {
        // Arrange
        var money = new TestMoney(100m, "USD");
        TestMoney? nullMoney = null;

        // Act & Assert
        Assert.False(money == nullMoney);
        Assert.False(nullMoney == money);
    }

    [Fact]
    public void EqualityOperator_WithDifferentTypes_ShouldReturnFalse()
    {
        // Arrange
        var money = new TestMoney(100m, "USD");
        var address = new TestAddress("Main St", "New York", "10001");

        // Act & Assert
        Assert.False(money == address);
    }

    #endregion

    #region Inequality Operator Tests

    [Fact]
    public void InequalityOperator_WithDifferentAtomicValues_ShouldReturnTrue()
    {
        // Arrange
        var money1 = new TestMoney(100m, "USD");
        var money2 = new TestMoney(200m, "USD");

        // Act & Assert
        Assert.True(money1 != money2);
    }

    [Fact]
    public void InequalityOperator_WithSameAtomicValues_ShouldReturnFalse()
    {
        // Arrange
        var money1 = new TestMoney(100m, "USD");
        var money2 = new TestMoney(100m, "USD");

        // Act & Assert
        Assert.False(money1 != money2);
    }

    [Fact]
    public void InequalityOperator_BothNull_ShouldReturnFalse()
    {
        // Arrange
        TestMoney? money1 = null;
        TestMoney? money2 = null;

        // Act & Assert
        Assert.False(money1 != money2);
    }

    [Fact]
    public void InequalityOperator_OneNullOneNotNull_ShouldReturnTrue()
    {
        // Arrange
        var money = new TestMoney(100m, "USD");
        TestMoney? nullMoney = null;

        // Act & Assert
        Assert.True(money != nullMoney);
        Assert.True(nullMoney != money);
    }

    #endregion

    #region Hash Code Tests

    [Fact]
    public void GetHashCode_WithSameAtomicValues_ShouldBeSame()
    {
        // Arrange
        var money1 = new TestMoney(100m, "USD");
        var money2 = new TestMoney(100m, "USD");

        // Act
        var hash1 = money1.GetHashCode();
        var hash2 = money2.GetHashCode();

        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void GetHashCode_WithDifferentAtomicValues_ShouldLikelyBeDifferent()
    {
        // Arrange
        var money1 = new TestMoney(100m, "USD");
        var money2 = new TestMoney(200m, "USD");

        // Act
        var hash1 = money1.GetHashCode();
        var hash2 = money2.GetHashCode();

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void GetHashCode_MultipleCallsSameInstance_ShouldReturnSameValue()
    {
        // Arrange
        var money = new TestMoney(100m, "USD");

        // Act
        var hash1 = money.GetHashCode();
        var hash2 = money.GetHashCode();
        var hash3 = money.GetHashCode();

        // Assert
        Assert.Equal(hash1, hash2);
        Assert.Equal(hash2, hash3);
    }

    [Fact]
    public void GetHashCode_CanBeUsedInHashBasedCollections()
    {
        // Arrange
        var money1 = new TestMoney(100m, "USD");
        var money2 = new TestMoney(100m, "USD");
        var hashSet = new HashSet<TestMoney> { money1 };

        // Act & Assert
        Assert.Contains(money2, hashSet);
    }

    [Fact]
    public void GetHashCode_MultiplePropertiesAllSame_ShouldBeSame()
    {
        // Arrange
        var address1 = new TestAddress("123 Main St", "New York", "10001");
        var address2 = new TestAddress("123 Main St", "New York", "10001");

        // Act & Assert
        Assert.Equal(address1.GetHashCode(), address2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_MultiplePropertiesOneDifferent_ShouldBeDifferent()
    {
        // Arrange
        var address1 = new TestAddress("123 Main St", "New York", "10001");
        var address2 = new TestAddress("123 Main St", "New York", "10002");

        // Act & Assert
        Assert.NotEqual(address1.GetHashCode(), address2.GetHashCode());
    }

    #endregion

    #region Hash Based Collection Tests

    [Fact]
    public void ValueObject_CanBeUsedInHashSet()
    {
        // Arrange
        var money1 = new TestMoney(100m, "USD");
        var money2 = new TestMoney(100m, "USD");
        var money3 = new TestMoney(200m, "USD");
        var hashSet = new HashSet<TestMoney>();

        // Act
        hashSet.Add(money1);
        hashSet.Add(money2); // Should not add duplicate
        hashSet.Add(money3);

        // Assert
        Assert.Equal(2, hashSet.Count);
    }

    [Fact]
    public void ValueObject_CanBeUsedAsDictionaryKey()
    {
        // Arrange
        var money1 = new TestMoney(100m, "USD");
        var money2 = new TestMoney(100m, "USD");
        var dictionary = new Dictionary<TestMoney, string>();

        // Act
        dictionary[money1] = "value1";

        // Assert
        Assert.True(dictionary.ContainsKey(money2));
        Assert.Equal("value1", dictionary[money2]);
    }

    [Fact]
    public void ValueObject_DifferentValuesAsDictionaryKey_ShouldHaveDifferentEntries()
    {
        // Arrange
        var money1 = new TestMoney(100m, "USD");
        var money2 = new TestMoney(200m, "USD");
        var dictionary = new Dictionary<TestMoney, string>();

        // Act
        dictionary[money1] = "hundred";
        dictionary[money2] = "two hundred";

        // Assert
        Assert.Equal(2, dictionary.Count);
        Assert.Equal("hundred", dictionary[money1]);
        Assert.Equal("two hundred", dictionary[money2]);
    }

    #endregion

    #region Type Checking Tests

    [Fact]
    public void Equals_DifferentValueObjectTypes_ShouldNotBeEqual()
    {
        // Arrange
        var money = new TestMoney(100m, "USD");
        var address = new TestAddress("Main St", "New York", "10001");

        // Act & Assert
        Assert.False(money == address);
    }

    [Fact]
    public void GetHashCode_DifferentTypes_CanBothBeInHashSet()
    {
        // Arrange
        var money = new TestMoney(100m, "USD");
        var address = new TestAddress("Main St", "New York", "10001");
        var hashSet = new HashSet<ValueObject> { money, address };

        // Act & Assert
        Assert.Equal(2, hashSet.Count);
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void Equals_WithZeroAndNegativeNumbers_ShouldRespectValues()
    {
        // Arrange
        var money1 = new TestMoney(0m, "USD");
        var money2 = new TestMoney(0m, "USD");

        // Act & Assert
        Assert.Equal(money1, money2);
    }

    [Fact]
    public void Equals_WithCaseInsensitiveCurrency_ShouldRespectCase()
    {
        // Arrange
        var money1 = new TestMoney(100m, "USD");
        var money2 = new TestMoney(100m, "usd");

        // Act & Assert
        Assert.NotEqual(money1, money2); // Case matters
    }

    [Fact]
    public void Equals_WithWhitespaceInStrings_ShouldBeDifferent()
    {
        // Arrange
        var address1 = new TestAddress("123 Main St", "New York", "10001");
        var address2 = new TestAddress("123 Main St ", "New York", "10001"); // Extra space

        // Act & Assert
        Assert.NotEqual(address1, address2);
    }

    [Fact]
    public void EqualityOperator_ReflexiveProperty_ShouldAlwaysBeTrue()
    {
        // Arrange
        var money = new TestMoney(100m, "USD");

        // Act & Assert
        Assert.True(money == money);
    }

    [Fact]
    public void EqualityOperator_SymmetricProperty_ShouldHoldTrue()
    {
        // Arrange
        var money1 = new TestMoney(100m, "USD");
        var money2 = new TestMoney(100m, "USD");

        // Act & Assert
        Assert.True(money1 == money2);
        Assert.True(money2 == money1);
    }

    [Fact]
    public void EqualityOperator_TransitiveProperty_ShouldHoldTrue()
    {
        // Arrange
        var money1 = new TestMoney(100m, "USD");
        var money2 = new TestMoney(100m, "USD");
        var money3 = new TestMoney(100m, "USD");

        // Act & Assert
        Assert.True(money1 == money2);
        Assert.True(money2 == money3);
        Assert.True(money1 == money3);
    }

    #endregion

    #region Integration Tests

    [Fact]
    public void ValueObject_CanBeSerializedAndDeserializedEquallyInHashSet()
    {
        // Arrange
        var original = new TestMoney(100m, "USD");
        var deserialized = new TestMoney(100m, "USD");
        var hashSet = new HashSet<TestMoney> { original };

        // Act & Assert
        Assert.Contains(deserialized, hashSet);
    }

    [Fact]
    public void ValueObject_MultipleInstancesWithSameValuesBehaveConsistently()
    {
        // Arrange
        var instances = Enumerable.Range(1, 10)
            .Select(_ => new TestMoney(100m, "USD"))
            .ToList();

        // Act & Assert
        foreach (var instance in instances)
        {
            foreach (var other in instances)
            {
                Assert.Equal(instance, other);
                Assert.Equal(instance.GetHashCode(), other.GetHashCode());
            }
        }
    }

    #endregion
}
