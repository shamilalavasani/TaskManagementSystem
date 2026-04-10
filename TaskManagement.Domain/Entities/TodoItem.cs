using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Entities;

public class TodoItem
{

    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    // public int? CreatedByUserId { get; private set; }
    public TodoItemStatus CompletionStatus { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime DueDate { get; private set; }
    public TodoPriority Priority { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;

    private TodoItem() { }// for EF Core

    public TodoItem(string title, string description, DateTime dueDate, Guid categoryId, TodoPriority priority = TodoPriority.Medium)
    {
        ValidateInputs(title, description, dueDate, categoryId);

        Id = Guid.NewGuid();

        this.Title = title;
        this.CreatedAt = DateTime.UtcNow;
        this.Description = description;
        CompletionStatus = TodoItemStatus.Pending;
        this.DueDate = dueDate;
        this.Priority = priority;
        this.CategoryId = categoryId;



    }
    private void ValidateInputs(string title, string description, DateTime dueDate, Guid categoryId)
    {

        if (string.IsNullOrWhiteSpace(title))

            throw new ArgumentException("Title cannot be empty.", nameof(title));


        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty.", nameof(description));


        if (dueDate <= DateTime.UtcNow)
            throw new ArgumentException("Due date must be in the future.", nameof(dueDate));


        //if (createdByUserId <= 0)
        //    throw new ArgumentException("CreatedByUserId must be a positive integer.", nameof(createdByUserId));

        if (categoryId == Guid.Empty)
            throw new ArgumentException("CategoryId must be a valid GUID.", nameof(categoryId));

    }

    public void UpdateDetails(string title, string description, DateTime dueDate, Guid categoryId, TodoPriority priority)
    {

        ValidateInputs(title, description, dueDate, categoryId);
        Title = title;
        Description = description;
        DueDate = dueDate;

        CategoryId = categoryId;
        Priority = priority;

    }

    public void ChangeStatus(TodoItemStatus newStatus)
    {
        if (CompletionStatus == newStatus)
            return;

        switch (CompletionStatus)
        {
            case TodoItemStatus.Pending:
                if (newStatus != TodoItemStatus.InProgress && newStatus != TodoItemStatus.Cancelled)
                    throw new InvalidOperationException("Invalid status transition from Pending.");
                break;

            case TodoItemStatus.InProgress:
                if (newStatus != TodoItemStatus.Completed && newStatus != TodoItemStatus.Cancelled)
                    throw new InvalidOperationException("Invalid status transition from InProgress.");
                break;

            case TodoItemStatus.Completed:
            case TodoItemStatus.Cancelled:
                throw new InvalidOperationException("Cannot change status once it is Completed or Cancelled.");
        }

        CompletionStatus = newStatus;
    }
    //these are ok:
    //Pending → InProgress
    //InProgress → Completed
    //Pending → Cancelled
    //InProgress → Cancelled

}