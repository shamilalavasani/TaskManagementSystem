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

    private TodoItem() { }// for EF Core

    public TodoItem(string title, string description, DateTime dueDate, TodoPriority priority = TodoPriority.Medium)
    {
        ValidateInputs(title, description, dueDate);

        Id = Guid.NewGuid();

        this.Title = title;
        this.CreatedAt = DateTime.UtcNow;
        this.Description = description;
        CompletionStatus = TodoItemStatus.Pending;
        this.DueDate = dueDate;
        this.Priority = priority;



    }
    private void ValidateInputs(string title, string description, DateTime dueDate)
    {

        if (string.IsNullOrWhiteSpace(title))

            throw new ArgumentException("Title cannot be empty.", nameof(title));


        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty.", nameof(description));


        if (dueDate <= DateTime.UtcNow)
            throw new ArgumentException("Due date must be in the future.", nameof(dueDate));


        //if (createdByUserId <= 0)
        //    throw new ArgumentException("CreatedByUserId must be a positive integer.", nameof(createdByUserId));


    }

    public void UpdateDetails(string title, string description, DateTime dueDate, TodoPriority priority)
    {

        ValidateInputs(title, description, dueDate);
        Title = title;
        Description = description;
        DueDate = dueDate;
        Priority = priority;
        // CompletionStatus = completionStatus;
    }

    public void ChangeStatus(TodoItemStatus status)
    {
        CompletionStatus = status;
    }


}