using AM.Codeflix.Catalog.Application.Interfaces;
using AM.Codeflix.Catalog.Application.UseCases.Category.Common;
using AM.Codeflix.Catalog.Domain.Repository;
using DomainEntity = AM.Codeflix.Catalog.Domain.Entity;

namespace AM.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;

public class CreateCategory(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork) : ICreateCategory
{
    public async Task<CategoryModelOutput> Handle(CreateCategoryInput input, CancellationToken cancellationToken)
    {
        var category = new DomainEntity.Category(
            input.Name,
            input.Description,
            input.IsActive
        );

        await categoryRepository.Insert(category, cancellationToken);

        await unitOfWork.Commit(cancellationToken);

        return CategoryModelOutput.FromCategory(category);
    }
}