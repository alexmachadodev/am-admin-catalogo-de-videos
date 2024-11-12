using AM.Codeflix.Catalog.Application.UseCases.Category.Common;

namespace AM.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;

public interface ICreateCategory
{
    public Task<CategoryModelOutput> Handle(CreateCategoryInput input, CancellationToken cancellationToken);
}