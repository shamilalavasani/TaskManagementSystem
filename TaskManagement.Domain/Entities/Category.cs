namespace TaskManagement.Domain.Entities;


public class Category
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public string? Description { get; private set; }

    public DateTime CreatedAt { get; private set; }

    // Navigation Property
    public ICollection<TodoItem> TodoItems { get; private set; } = new List<TodoItem>();


    // Constructor اصلی (برای ساخت توسط برنامه)
    public Category(string name, string? description)
    {
        Id = Guid.NewGuid();
        SetName(name);
        SetDescription(description);
        CreatedAt = DateTime.UtcNow;
    }

    // Constructor خالی (برای EF Core)
    private Category() { }


    // Methods (Domain Behavior)

    public void Update(string name, string? description)
    {
        SetName(name);
        SetDescription(description);
    }

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name cannot be empty.", nameof(name));

        Name = name;
    }

    private void SetDescription(string? description)
    {
        Description = description;
    }
}