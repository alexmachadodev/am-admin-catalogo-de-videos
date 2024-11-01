using FluentAssertions;
using AM.Codeflix.Catalog.Domain.Exceptions;
using DomainEntity = AM.Codeflix.Catalog.Domain.Entity;

namespace AM.Codeflix.Catalog.UnitTests.Domain.Entity.Category;

[Collection(nameof(CategoryTestFixture))]
public class CategoryTest(CategoryTestFixture categoryTestFixture)
{
    [Fact(DisplayName = nameof(Instantiate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Instantiate()
    {
        // Arrange
        var validCategory = categoryTestFixture.GetValidCategory();
        var datetimeBefore = DateTime.Now;

        // Act
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        // Assert
        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default);
        (category.CreatedAt >= datetimeBefore).Should().BeTrue();
        (category.CreatedAt <= datetimeAfter).Should().BeTrue();
        category.IsActive.Should().BeTrue();
    }

    [Theory(DisplayName = nameof(InstantiateWithIsActive))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData(true)]
    [InlineData(false)]
    public void InstantiateWithIsActive(bool isActive)
    {
        // Arrange
        var validCategory = categoryTestFixture.GetValidCategory();
        var datetimeBefore = DateTime.Now;

        // Act
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, isActive);
        var datetimeAfter = DateTime.Now.AddSeconds(1);

        // Assert
        category.Should().NotBeNull();
        category.Name.Should().Be(validCategory.Name);
        category.Description.Should().Be(validCategory.Description);
        category.Id.Should().NotBeEmpty();
        category.CreatedAt.Should().NotBeSameDateAs(default);
        (category.CreatedAt >= datetimeBefore).Should().BeTrue();
        (category.CreatedAt <= datetimeAfter).Should().BeTrue();
        category.IsActive.Should().Be(isActive);
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public void InstantiateErrorWhenNameIsEmpty(string? name)
    {
        // Act
        var action = () => new DomainEntity.Category(name!, categoryTestFixture.GetValidCategoryDescription());

        // Assert
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsNull))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsNull()
    {
        // Act
        var action = () => new DomainEntity.Category(categoryTestFixture.GetValidCategoryName(), null!);

        // Assert
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Description should not be null");
    }

    [Theory(DisplayName = nameof(InstantiateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("1")]
    [InlineData("12")]
    [InlineData("a")]
    [InlineData("ab")]
    public void InstantiateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        var action = () => _ = new DomainEntity.Category(invalidName, categoryTestFixture.GetValidCategoryDescription());

        // Assert
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should be at leats 3 characters long");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenNameIsGreaterThan255Characters()
    {
        // Arrange
        var validCategory = categoryTestFixture.GetValidCategory();
        var invalidName = categoryTestFixture.Faker.Lorem.Letter(256);

        // Act
        var action = () => new DomainEntity.Category(invalidName, validCategory.Description);

        // Assert
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should not be greater than 255 characters long");
    }

    [Fact(DisplayName = nameof(InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void InstantiateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        // Arrange
        var validCategory = categoryTestFixture.GetValidCategory();
        var invalidDescription = categoryTestFixture.Faker.Commerce.ProductDescription();
        while (invalidDescription.Length <= 10_000)
            invalidDescription = $"{invalidDescription} {categoryTestFixture.Faker.Commerce.ProductDescription()}";

        // Act
        var action = () => new DomainEntity.Category(validCategory.Name, invalidDescription);

        // Assert
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Description should not be greater than 10000 characters long");
    }


    [Fact(DisplayName = nameof(Activate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Activate()
    {
        // Arrange
        var validCategory = categoryTestFixture.GetValidCategory();

        // Act
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, false);
        category.Activate();

        // Assert
        category.IsActive.Should().BeTrue();
    }

    [Fact(DisplayName = nameof(Deactivate))]
    [Trait("Domain", "Category - Aggregates")]
    public void Deactivate()
    {
        // Arrange
        var validCategory = categoryTestFixture.GetValidCategory();
        var category = new DomainEntity.Category(validCategory.Name, validCategory.Description, true);

        // Act
        category.Deactivate();

        // Assert
        category.IsActive.Should().BeFalse();
    }

    [Fact(DisplayName = nameof(Update))]
    [Trait("Domain", "Category - Aggregates")]
    public void Update()
    {
        // Arrange
        var validCategory = categoryTestFixture.GetValidCategory();
        var newValues = categoryTestFixture.GetValidCategory();

        // Act
        validCategory.Update(newValues.Name, newValues.Description);

        // Assert
        validCategory.Name.Should().Be(newValues.Name);
        validCategory.Description.Should().Be(newValues.Description);
    }

    [Fact(DisplayName = nameof(UpdateOnlyName))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateOnlyName()
    {
        // Arrange
        var validCategory = categoryTestFixture.GetValidCategory();
        var newName = categoryTestFixture.GetValidCategoryName();
        var currentDescription = validCategory.Description;

        // Act
        validCategory.Update(newName);

        // Assert
        validCategory.Name.Should().Be(newName);
        validCategory.Description.Should().Be(currentDescription);
    }

    [Theory(DisplayName = nameof(UdpateErrorWhenNameIsEmpty))]
    [Trait("Domain", "Category - Aggregates")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("  ")]
    public void UdpateErrorWhenNameIsEmpty(string? name)
    {
        // Act
        var validCategory = categoryTestFixture.GetValidCategory();
        var action = () => validCategory.Update(name!);

        // Assert
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should not be empty or null");
    }

    [Theory(DisplayName = nameof(UpdateErrorWhenNameIsLessThan3Characters))]
    [Trait("Domain", "Category - Aggregates")]
    [MemberData(nameof(GetNamesWithLessThan3Characters), parameters:10)]
    public void UpdateErrorWhenNameIsLessThan3Characters(string invalidName)
    {
        // Arrange
        var validCategory = categoryTestFixture.GetValidCategory();

        // Act
        var action = () => validCategory.Update(invalidName);

        // Assert
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should be at leats 3 characters long");
    }

    public static IEnumerable<object[]> GetNamesWithLessThan3Characters(int numberOfTests)
    {
        var fixture = new CategoryTestFixture();

        for (var i = 0; i < numberOfTests; i++)
        {
            var isOdd = i % 2 == 1;
            yield return [fixture.GetValidCategoryName()[..(isOdd ? 1 : 2)]];
        }
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenNameIsGreaterThan255Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenNameIsGreaterThan255Characters()
    {
        // Arrange
        var validCategory = categoryTestFixture.GetValidCategory();
        var invalidName = categoryTestFixture.Faker.Lorem.Letter(256);

        // Act
        var action = () => validCategory.Update(invalidName);

        // Assert
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Name should not be greater than 255 characters long");
    }

    [Fact(DisplayName = nameof(UpdateErrorWhenDescriptionIsGreaterThan10_000Characters))]
    [Trait("Domain", "Category - Aggregates")]
    public void UpdateErrorWhenDescriptionIsGreaterThan10_000Characters()
    {
        // Arrange
        var validCategory = categoryTestFixture.GetValidCategory();
        var invalidDescription = categoryTestFixture.Faker.Commerce.ProductDescription();
        while (invalidDescription.Length <= 10_000)
            invalidDescription = $"{invalidDescription} {categoryTestFixture.Faker.Commerce.ProductDescription()}";

        // Act
        var action = () => validCategory.Update(categoryTestFixture.GetValidCategoryName(), invalidDescription);

        // Assert
        action.Should().Throw<EntityValidationException>()
            .WithMessage("Description should not be greater than 10000 characters long");
    }
}