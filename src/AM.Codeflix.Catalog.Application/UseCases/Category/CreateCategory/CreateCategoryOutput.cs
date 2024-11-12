namespace AM.Codeflix.Catalog.Application.UseCases.Category.CreateCategory;

public class CreateCategoryOutput
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}