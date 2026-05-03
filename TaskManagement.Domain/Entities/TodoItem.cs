using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Entities;

public class TodoItem
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public string OwnerUserId { get; private set; } = string.Empty;
    public TodoItemStatus CompletionStatus { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime DueDate { get; private set; }
    public TodoPriority Priority { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; } = null!;

    private TodoItem() { }// for EF Core

    public TodoItem(string title, string description, DateTime dueDate, Guid categoryId, string ownerUserId, TodoPriority priority = TodoPriority.Medium)
    {
        ValidateInputs(title, description, dueDate, categoryId);
        if (string.IsNullOrWhiteSpace(ownerUserId))
            throw new ArgumentException("ownerUserId can not be empty.", nameof(ownerUserId));

        Id = Guid.NewGuid();

        this.Title = title;
        this.CreatedAt = DateTime.UtcNow;
        this.Description = description;
        CompletionStatus = TodoItemStatus.Pending;
        this.DueDate = dueDate;
        this.Priority = priority;
        this.CategoryId = categoryId;
        this.OwnerUserId = ownerUserId;


    }

    private void ValidateInputs(string title, string description, DateTime dueDate, Guid categoryId)
    {

        if (string.IsNullOrWhiteSpace(title))

            throw new ArgumentException("Title cannot be empty.", nameof(title));


        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be empty.", nameof(description));


        if (dueDate <= DateTime.UtcNow)
            throw new ArgumentException("Due date must be in the future.", nameof(dueDate));




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
        if (!CanChangeStatusTo(newStatus))
            throw new InvalidOperationException(
                $"Cannot change status from {CompletionStatus} to {newStatus}.");

        CompletionStatus = newStatus;
    }
    public bool CanChangeStatusTo(TodoItemStatus newStatus)
    {
        if (CompletionStatus == newStatus)
            return true;

        return CompletionStatus switch
        {
            TodoItemStatus.Pending =>
                newStatus is TodoItemStatus.InProgress or TodoItemStatus.Cancelled,

            TodoItemStatus.InProgress =>
                newStatus is TodoItemStatus.Completed or TodoItemStatus.Cancelled,

            TodoItemStatus.Completed or TodoItemStatus.Cancelled =>
                false,

            _ => false
        };
    }


}