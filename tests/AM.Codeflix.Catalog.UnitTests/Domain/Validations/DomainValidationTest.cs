using Bogus;
using FluentAssertions;
using AM.Codeflix.Catalog.Domain.Exceptions;
using AM.Codeflix.Catalog.Domain.Validations;

namespace AM.Codeflix.Catalog.UnitTests.Domain.Validations;

public class DomainValidationTest
{
    private Faker Faker { get; } = new Faker();
    
    [Fact(DisplayName = nameof(NotNullOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOk()
    {
        var value = Faker.Commerce.ProductName();
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        var action = () => DomainValidation.NotNull(value, fieldName);

        action.Should().NotThrow();
    }

    [Fact(DisplayName = nameof(NotNullThrowWhenNull))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullThrowWhenNull()
    {
        string? value = null;
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        var action = () => DomainValidation.NotNull(value, fieldName);

        action.Should().Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should not be null");
    }


    [Theory(DisplayName = nameof(NotNullOrEmptyThrowWhenEmpty))]
    [Trait("Domain", "DomainValidation - Validation")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public void NotNullOrEmptyThrowWhenEmpty(string? target)
    {
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        var action = () => DomainValidation.NotNullOrEmpty(target, fieldName);

        action.Should().Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should not be empty or null");
    }

    [Fact(DisplayName = nameof(NotNullOrEmptyOk))]
    [Trait("Domain", "DomainValidation - Validation")]
    public void NotNullOrEmptyOk()
    {
        var value = Faker.Commerce.ProductName();
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");

        var action = () => DomainValidation.NotNullOrEmpty(value, fieldName);

        action.Should().NotThrow();
    }

    [Theory(DisplayName = nameof(MinLengthThrowWhenLess))]
    [Trait("Domain", "DomainValidation - Validation")]
    [MemberData(nameof(GetValuesSmallerThan), parameters: 10)]
    public void MinLengthThrowWhenLess(string target, int minLength)
    {
        var fieldName = Faker.Commerce.ProductName().Replace(" ", "");
        var action = () => DomainValidation.MinLength(target, minLength, fieldName);

        action.Should().Throw<EntityValidationException>()
            .WithMessage($"{fieldName} should be at least {minLength} characters long");
    }

    public static IEnumerable<object[]> GetValuesSmallerThan(int numberOfTests = 5)
    {
        yield return ["123456", 10];

        var faker = new Faker();

        for (var i = 0; i < numberOfTests - 1; i++)
        {
            var example = faker.Commerce.ProductName();
            var minLength = example.Length + (new Random()).Next(1, 20);

            yield return [example, minLength];
        }
    }
}