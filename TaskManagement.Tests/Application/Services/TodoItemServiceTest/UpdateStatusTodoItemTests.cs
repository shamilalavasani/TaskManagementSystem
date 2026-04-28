
using FluentAssertions;
using Moq;
using TaskManagement.Application.Common.Exceptions;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Tests.Application.Services.TodoItemServiceTest;

public class UpdateStatusTodoItemTests : TodoItemServiceTestBase
{
    [Fact]
    public async Task UpdateStatusTodoItemAsync_ShouldUpdateStatus_WhenUserIsOwner()
    {
        // Arrange
        var (repoMock, categoryMock, service) = CreateServiceWithMocks();

        var todoId = Guid.NewGuid();
        var ownerUserId = "owner-user-id";

        var existingTodo = CreateTodo(
            title: "Test Title",
            description: "Test Description",
            priority: TodoPriority.Medium,
            ownerId: ownerUserId,
            categoryId: Guid.NewGuid());

        repoMock
            .Setup(r => r.GetByIdAsync(todoId))
            .ReturnsAsync(existingTodo);

        // Act
        await service.UpdateStatusTodoItemAsync(
            todoId,
            TodoItemStatus.InProgress,
            ownerUserId,
            isAdminOrManager: false);

        // Assert
        existingTodo.CompletionStatus.Should().Be(TodoItemStatus.InProgress);

        repoMock.Verify(r => r.GetByIdAsync(todoId), Times.Once);
        repoMock.Verify(r => r.UpdateAsync(existingTodo), Times.Once);
    }

    [Fact]
    public async Task UpdateStatusTodoItemAsync_ShouldUpdateStatus_WhenUserIsAdminOrManager()
    {
        // Arrange
        var (repoMock, categoryMock, service) = CreateServiceWithMocks();

        var todoId = Guid.NewGuid();

        var existingTodo = CreateTodo(
            title: "Test Title",
            description: "Test Description",
            priority: TodoPriority.Medium,
            ownerId: "real-owner-id",
            categoryId: Guid.NewGuid());

        repoMock
            .Setup(r => r.GetByIdAsync(todoId))
            .ReturnsAsync(existingTodo);

        // Act
        await service.UpdateStatusTodoItemAsync(
            todoId,
            TodoItemStatus.InProgress,
            userId: "another-user-id",
            isAdminOrManager: true);

        // Assert
        existingTodo.CompletionStatus.Should().Be(TodoItemStatus.InProgress);

        repoMock.Verify(r => r.GetByIdAsync(todoId), Times.Once);
        repoMock.Verify(r => r.UpdateAsync(existingTodo), Times.Once);
    }
    [Fact]
    public async Task UpdateStatusTodoItemAsync_ShouldThrowNotFoundException_WhenTodoDoesNotExist()
    {
        // Arrange
        var (repoMock, categoryMock, service) = CreateServiceWithMocks();

        var todoId = Guid.NewGuid();

        repoMock
            .Setup(r => r.GetByIdAsync(todoId))
            .ReturnsAsync((TodoItem?)null);

        // Act
        Func<Task> act = async () =>
            await service.UpdateStatusTodoItemAsync(
                todoId,
                TodoItemStatus.InProgress,
                userId: "user-id",
                isAdminOrManager: false);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();

        repoMock.Verify(r => r.UpdateAsync(It.IsAny<TodoItem>()), Times.Never);
    }
    [Fact]
    public async Task UpdateStatusTodoItemAsync_ShouldThrowForbiddenException_WhenUserIsNotOwnerAndNotAdminOrManager()
    {
        // Arrange
        var (repoMock, categoryMock, service) = CreateServiceWithMocks();

        var todoId = Guid.NewGuid();

        var existingTodo = CreateTodo(
            title: "Test Title",
            description: "Test Description",
            priority: TodoPriority.Medium,
            ownerId: "real-owner-id",
            categoryId: Guid.NewGuid());

        repoMock
            .Setup(r => r.GetByIdAsync(todoId))
            .ReturnsAsync(existingTodo);

        // Act
        Func<Task> act = async () =>
            await service.UpdateStatusTodoItemAsync(
                todoId,
                TodoItemStatus.InProgress,
                userId: "another-user-id",
                isAdminOrManager: false);

        // Assert
        await act.Should().ThrowAsync<ForbiddenException>();

        repoMock.Verify(r => r.UpdateAsync(It.IsAny<TodoItem>()), Times.Never);
    }
    [Fact]
    public async Task UpdateStatusTodoItemAsync_ShouldThrowInvalidOperationException_WhenStatusTransitionIsInvalid()
    {
        // Arrange
        var (repoMock, categoryMock, service) = CreateServiceWithMocks();

        var todoId = Guid.NewGuid();
        var ownerUserId = "owner-user-id";

        var existingTodo = CreateTodo(
            title: "Test Title",
            description: "Test Description",
            priority: TodoPriority.Medium,
            ownerId: ownerUserId,
            categoryId: Guid.NewGuid());

        repoMock
            .Setup(r => r.GetByIdAsync(todoId))
            .ReturnsAsync(existingTodo);

        // Act
        Func<Task> act = async () =>
            await service.UpdateStatusTodoItemAsync(
                todoId,
                TodoItemStatus.Completed, // Pending -> Completed is invalid
                ownerUserId,
                isAdminOrManager: false);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();

        existingTodo.CompletionStatus.Should().Be(TodoItemStatus.Pending);
        repoMock.Verify(r => r.UpdateAsync(It.IsAny<TodoItem>()), Times.Never);
    }
    [Fact]
    public async Task UpdateStatusTodoItemAsync_ShouldAllowCancelling_WhenStatusIsPending()
    {
        // Arrange
        var (repoMock, categoryMock, service) = CreateServiceWithMocks();

        var todoId = Guid.NewGuid();
        var ownerUserId = "owner-user-id";

        var existingTodo = CreateTodo(
            title: "Test Title",
            description: "Test Description",
            priority: TodoPriority.Medium,
            ownerId: ownerUserId,
            categoryId: Guid.NewGuid());

        repoMock
            .Setup(r => r.GetByIdAsync(todoId))
            .ReturnsAsync(existingTodo);

        // Act
        await service.UpdateStatusTodoItemAsync(
            todoId,
            TodoItemStatus.Cancelled,
            ownerUserId,
            isAdminOrManager: false);

        // Assert
        existingTodo.CompletionStatus.Should().Be(TodoItemStatus.Cancelled);
        repoMock.Verify(r => r.UpdateAsync(existingTodo), Times.Once);
    }
    [Fact]
    public async Task UpdateStatusTodoItemAsync_ShouldAllowCompleting_WhenStatusIsInProgress()
    {
        // Arrange
        var (repoMock, categoryMock, service) = CreateServiceWithMocks();

        var todoId = Guid.NewGuid();
        var ownerUserId = "owner-user-id";

        var existingTodo = CreateTodo(
            title: "Test Title",
            description: "Test Description",
            priority: TodoPriority.Medium,
            ownerId: ownerUserId,
            categoryId: Guid.NewGuid());

        existingTodo.ChangeStatus(TodoItemStatus.InProgress);

        repoMock
            .Setup(r => r.GetByIdAsync(todoId))
            .ReturnsAsync(existingTodo);

        // Act
        await service.UpdateStatusTodoItemAsync(
            todoId,
            TodoItemStatus.Completed,
            ownerUserId,
            isAdminOrManager: false);

        // Assert
        existingTodo.CompletionStatus.Should().Be(TodoItemStatus.Completed);
        repoMock.Verify(r => r.UpdateAsync(existingTodo), Times.Once);
    }
    [Fact]
    public async Task UpdateStatusTodoItemAsync_ShouldThrowInvalidOperationException_WhenTryingToChangeCompletedTodo()
    {
        // Arrange
        var (repoMock, categoryMock, service) = CreateServiceWithMocks();

        var todoId = Guid.NewGuid();
        var ownerUserId = "owner-user-id";

        var existingTodo = CreateTodo(
            title: "Test Title",
            description: "Test Description",
            priority: TodoPriority.Medium,
            ownerId: ownerUserId,
            categoryId: Guid.NewGuid());

        existingTodo.ChangeStatus(TodoItemStatus.InProgress);
        existingTodo.ChangeStatus(TodoItemStatus.Completed);

        repoMock
            .Setup(r => r.GetByIdAsync(todoId))
            .ReturnsAsync(existingTodo);

        // Act
        Func<Task> act = async () =>
            await service.UpdateStatusTodoItemAsync(
                todoId,
                TodoItemStatus.Cancelled,
                ownerUserId,
                isAdminOrManager: false);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>();

        existingTodo.CompletionStatus.Should().Be(TodoItemStatus.Completed);
        repoMock.Verify(r => r.UpdateAsync(It.IsAny<TodoItem>()), Times.Never);
    }
}
