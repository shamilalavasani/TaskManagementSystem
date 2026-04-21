using FluentAssertions;
using Moq;
using TaskManagement.Application.Common.Exceptions;
using TaskManagement.Application.DTOs.TodoItemDTOs;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Tests.Application.Services.TodoItemServiceTest;

public class UpdateTodoItemTests : TodoItemServiceTestBase
{
    [Fact]
    public async Task UpdateTodoItemAsync_ShouldUpdateTodo_WhenRequestIsValid()
    {
        // Arrange
        var (repoMock, categoryMock, service) = CreateServiceWithMocks();

        var todoId = Guid.NewGuid();
        var newCategoryId = Guid.NewGuid();
        var ownerUserId = "owner-user-id";

        var existingTodo = CreateTodo(
            title: "Old Title",
            description: "Old Description",
            priority: TodoPriority.Low,
            ownerId: ownerUserId,
            categoryId: Guid.NewGuid());

        var updateDto = new UpdateTodoItemDto
        {
            Title = "Updated Title",
            Description = "Updated Description",
            DueDate = DateTime.UtcNow.AddDays(3),
            Priority = TodoPriority.High,
            CategoryId = newCategoryId
        };

        repoMock
            .Setup(r => r.GetByIdAsync(todoId))
            .ReturnsAsync(existingTodo);

        categoryMock
            .Setup(c => c.ExistsAsync(newCategoryId))
            .ReturnsAsync(true);

        // Act
        await service.UpdateTodoItemAsync(
        todoId,
        updateDto,
        ownerUserId,
        isAdminOrManager: false);

        // Assert
        existingTodo.Title.Should().Be("Updated Title");
        existingTodo.Description.Should().Be("Updated Description");
        existingTodo.Priority.Should().Be(TodoPriority.High);
        existingTodo.CategoryId.Should().Be(newCategoryId);
        existingTodo.DueDate.Should().Be(updateDto.DueDate);

        repoMock.Verify(r => r.UpdateAsync(existingTodo), Times.Once);
        repoMock.Verify(r => r.GetByIdAsync(todoId), Times.Once);
    }

    [Fact]
    public async Task UpdateTodoItemAsync_ShouldThrowNotFoundException_WhenTodoDoesNotExist()
    {
        // Arrange
        var (repoMock, categoryMock, service) = CreateServiceWithMocks();

        var todoId = Guid.NewGuid();

        var updateDto = new UpdateTodoItemDto
        {
            Title = "Updated Title",
            Description = "Updated Description",
            DueDate = DateTime.UtcNow.AddDays(2),
            Priority = TodoPriority.High,
            CategoryId = Guid.NewGuid()
        };

        repoMock
            .Setup(r => r.GetByIdAsync(todoId))
            .ReturnsAsync((TodoItem?)null);

        // Act
        Func<Task> act = async () =>
            await service.UpdateTodoItemAsync(
                todoId,
                updateDto,
                "owner-user-id",
                isAdminOrManager: false);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();

        categoryMock.Verify(c => c.ExistsAsync(It.IsAny<Guid>()), Times.Never);
        repoMock.Verify(r => r.UpdateAsync(It.IsAny<TodoItem>()), Times.Never);
    }
    [Fact]
    public async Task UpdateTodoItemAsync_ShouldThrowForbiddenException_WhenUserIsNotOwnerAndNotAdminOrManager()
    {
        // Arrange
        var (repoMock, categoryMock, service) = CreateServiceWithMocks();

        var todoId = Guid.NewGuid();

        var existingTodo = CreateTodo(
            title: "Old Title",
            description: "Old Description",
            priority: TodoPriority.Low,
            ownerId: "real-owner-id",
            categoryId: Guid.NewGuid());

        var updateDto = new UpdateTodoItemDto
        {
            Title = "Updated Title",
            Description = "Updated Description",
            DueDate = DateTime.UtcNow.AddDays(2),
            Priority = TodoPriority.High,
            CategoryId = Guid.NewGuid()
        };

        repoMock
            .Setup(r => r.GetByIdAsync(todoId))
            .ReturnsAsync(existingTodo);

        // Act
        Func<Task> act = async () =>
            await service.UpdateTodoItemAsync(
                todoId,
                updateDto,
                "another-user-id",
                isAdminOrManager: false);

        // Assert
        await act.Should().ThrowAsync<ForbiddenException>();

        categoryMock.Verify(c => c.ExistsAsync(It.IsAny<Guid>()), Times.Never);
        repoMock.Verify(r => r.UpdateAsync(It.IsAny<TodoItem>()), Times.Never);
    }
    [Fact]
    public async Task UpdateTodoItemAsync_ShouldThrowNotFoundException_WhenCategoryDoesNotExist()
    {
        // Arrange
        var (repoMock, categoryMock, service) = CreateServiceWithMocks();

        var todoId = Guid.NewGuid();
        var ownerUserId = "owner-user-id";
        var missingCategoryId = Guid.NewGuid();

        var existingTodo = CreateTodo(
            title: "Old Title",
            description: "Old Description",
            priority: TodoPriority.Low,
            ownerId: ownerUserId,
            categoryId: Guid.NewGuid());

        var updateDto = new UpdateTodoItemDto
        {
            Title = "Updated Title",
            Description = "Updated Description",
            DueDate = DateTime.UtcNow.AddDays(2),
            Priority = TodoPriority.High,
            CategoryId = missingCategoryId
        };

        repoMock
            .Setup(r => r.GetByIdAsync(todoId))
            .ReturnsAsync(existingTodo);

        categoryMock
            .Setup(c => c.ExistsAsync(missingCategoryId))
            .ReturnsAsync(false);

        // Act
        Func<Task> act = async () =>
            await service.UpdateTodoItemAsync(
                todoId,
                updateDto,
                ownerUserId,
                isAdminOrManager: false);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();

        repoMock.Verify(r => r.UpdateAsync(It.IsAny<TodoItem>()), Times.Never);
        categoryMock.Verify(c => c.ExistsAsync(missingCategoryId), Times.Once);
    }
    [Fact]
    public async Task UpdateTodoItemAsync_ShouldAllowUpdate_WhenUserIsAdminOrManager()
    {
        // Arrange
        var (repoMock, categoryMock, service) = CreateServiceWithMocks();

        var todoId = Guid.NewGuid();
        var newCategoryId = Guid.NewGuid();

        var existingTodo = CreateTodo(
            title: "Old Title",
            description: "Old Description",
            priority: TodoPriority.Low,
            ownerId: "real-owner-id",
            categoryId: Guid.NewGuid());

        var updateDto = new UpdateTodoItemDto
        {
            Title = "Updated By Admin",
            Description = "Changed Description",
            DueDate = DateTime.UtcNow.AddDays(4),
            Priority = TodoPriority.High,
            CategoryId = newCategoryId
        };

        repoMock
            .Setup(r => r.GetByIdAsync(todoId))
            .ReturnsAsync(existingTodo);

        categoryMock
            .Setup(c => c.ExistsAsync(newCategoryId))
            .ReturnsAsync(true);

        // Act
        await service.UpdateTodoItemAsync(
            todoId,
            updateDto,
            "another-user-id",
            isAdminOrManager: true);

        // Assert
        existingTodo.Title.Should().Be("Updated By Admin");
        existingTodo.Description.Should().Be("Changed Description");
        existingTodo.DueDate.Should().Be(updateDto.DueDate);
        existingTodo.Priority.Should().Be(TodoPriority.High);
        existingTodo.CategoryId.Should().Be(newCategoryId);

        repoMock.Verify(r => r.UpdateAsync(existingTodo), Times.Once);
    }
    [Fact]
    public async Task UpdateTodoItemAsync_ShouldPassUpdatedEntityToRepository()
    {
        // Arrange
        var (repoMock, categoryMock, service) = CreateServiceWithMocks();

        var todoId = Guid.NewGuid();
        var ownerUserId = "owner-user-id";
        var newCategoryId = Guid.NewGuid();

        var existingTodo = CreateTodo(
            title: "Old Title",
            description: "Old Description",
            priority: TodoPriority.Low,
            ownerId: ownerUserId,
            categoryId: Guid.NewGuid());

        var updateDto = new UpdateTodoItemDto
        {
            Title = "New Title",
            Description = "New Description",
            DueDate = DateTime.UtcNow.AddDays(5),
            Priority = TodoPriority.Medium,
            CategoryId = newCategoryId
        };

        repoMock
            .Setup(r => r.GetByIdAsync(todoId))
            .ReturnsAsync(existingTodo);

        categoryMock
            .Setup(c => c.ExistsAsync(newCategoryId))
            .ReturnsAsync(true);

        // Act
        await service.UpdateTodoItemAsync(
            todoId,
            updateDto,
            ownerUserId,
            isAdminOrManager: false);

        // Assert
        repoMock.Verify(r => r.UpdateAsync(It.Is<TodoItem>(t =>
            t.Title == updateDto.Title &&
            t.Description == updateDto.Description &&
            t.DueDate == updateDto.DueDate &&
            t.Priority == updateDto.Priority &&
            t.CategoryId == updateDto.CategoryId
        )), Times.Once);
    }
    [Fact]
    public async Task UpdateTodoItemAsync_ShouldThrowArgumentException_WhenDueDateIsNotInFuture()
    {
        // Arrange
        var (repoMock, categoryMock, service) = CreateServiceWithMocks();

        var todoId = Guid.NewGuid();
        var ownerUserId = "owner-user-id";
        var categoryId = Guid.NewGuid();

        var existingTodo = CreateTodo(
            title: "Old Title",
            description: "Old Description",
            priority: TodoPriority.Low,
            ownerId: ownerUserId,
            categoryId: categoryId);

        var updateDto = new UpdateTodoItemDto
        {
            Title = "Updated Title",
            Description = "Updated Description",
            DueDate = DateTime.UtcNow.AddMinutes(-5),
            Priority = TodoPriority.High,
            CategoryId = categoryId
        };

        repoMock
            .Setup(r => r.GetByIdAsync(todoId))
            .ReturnsAsync(existingTodo);

        categoryMock
            .Setup(c => c.ExistsAsync(categoryId))
            .ReturnsAsync(true);

        // Act
        Func<Task> act = async () =>
            await service.UpdateTodoItemAsync(
                todoId,
                updateDto,
                ownerUserId,
                isAdminOrManager: false);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();

        repoMock.Verify(r => r.UpdateAsync(It.IsAny<TodoItem>()), Times.Never);
    }

}
