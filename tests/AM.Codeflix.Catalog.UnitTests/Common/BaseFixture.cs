using Bogus;

namespace AM.Codeflix.Catalog.UnitTests.Common;

public abstract class BaseFixture
{
    public Faker Faker { get; } = new("pt_BR");
}