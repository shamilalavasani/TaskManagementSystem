using FluentAssertions;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Entities.TodoItemTests;

public class TodoItemTests
{
    [Fact]
    public void CanChangeStatusTo_ShouldReturnTrue_WhenChangingFromPendingToInProgress()
    {
        var todo = CreateTodo();
        var result = todo.CanChangeStatusTo(TodoItemStatus.InProgress);
        result.Should().BeTrue();
    }
    [Fact]
    public void CanChangeStatusTo_ShouldReturnFalse_WhenChangingFromPendingToCompleted()
    {
        var todo = CreateTodo();
        var result = todo.CanChangeStatusTo(TodoItemStatus.Completed);
        result.Should().BeFalse();
    }
    [Fact]
    public void ChangeStatus_ShouldUpdateStatus_WhenTransitionIsValid()
    {
        var todo = CreateTodo();
        todo.ChangeStatus(TodoItemStatus.InProgress);
        todo.CompletionStatus.Should().Be(TodoItemStatus.InProgress);
    }
    [Fact]
    public void ChangeStatus_ShouldThrowInvalidOperationException_WhenTransitionIsInvalid()
    {
        var todo = CreateTodo();

        Action act = () => todo.ChangeStatus(TodoItemStatus.Completed);

        act.Should().Throw<InvalidOperationException>();

        todo.CompletionStatus.Should().Be(TodoItemStatus.Pending);
    }
    [Fact]
    public void ChangeStatus_ShouldThrowException_WhenStatusIsAlreadyCompleted()
    {

        var todo = CreateTodo();
        todo.ChangeStatus(TodoItemStatus.InProgress);
        todo.ChangeStatus(TodoItemStatus.Completed);
        //changeStatus is not an async method .
        Action act = () => todo.ChangeStatus(TodoItemStatus.Cancelled);

        act.Should().Throw<InvalidOperationException>();

        todo.CompletionStatus.Should().Be(TodoItemStatus.Completed);
    }
    [Fact]
    public void ChangeStatus_ShouldAllowCancel_WhenStatusIsPending()
    {
        var todo = CreateTodo();
        todo.ChangeStatus(TodoItemStatus.Cancelled);
        todo.CompletionStatus.Should().Be(TodoItemStatus.Cancelled);
    }
    private static TodoItem CreateTodo()
    {
        return new TodoItem(
            "Test Title",
            "Test Description",
            DateTime.UtcNow.AddDays(1),
            Guid.NewGuid(),
            "owner-user-id",
            TodoPriority.Medium);
    }
}
