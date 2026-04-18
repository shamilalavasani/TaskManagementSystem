using Moq;
using TaskManagement.Application.Common.Exceptions;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Tests.Application.Services.TodoItemServiceTest;

public class GetTodoItemByIdTests : TodoItemServiceTestBase
{
    [Fact]
    public async Task GetTodoItemById_ShouldThrowNotFound_WhenTodoDoesNotExist()
    {
        // Arrange
        var (repoMock, _, service) = CreateServiceWithMocks();
        repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((TodoItem?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.GetTodoItemByIdAsync(Guid.NewGuid(), "user1", false));
    }
    [Fact]
    public async Task GetTodoItemById_ShouldThrowForbidden_WhenUserIsNotOwner()
    {
        // Arrange
        var (repoMock, _, service) = CreateServiceWithMocks();
        var todo = CreateTodo("Test Title", "Test Description", TodoPriority.Medium, "owner-user-id");

        repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(todo);

        // Act & Assert
        await Assert.ThrowsAsync<ForbiddenException>(() =>
            service.GetTodoItemByIdAsync(Guid.NewGuid(), "other-user-id", false));
    }
    [Fact]
    public async Task GetTodoItemById_ShouldReturnTodo_WhenUserIsOwner()
    {
        // Arrange

        var (repoMock, _, service) = CreateServiceWithMocks();
        var categoryId = Guid.NewGuid();
        var todo = CreateTodo("Test Title", "Test Description", TodoPriority.High, "owner-user-id", categoryId);

        repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(todo);

        // Act
        var result = await service.GetTodoItemByIdAsync(Guid.NewGuid(), "owner-user-id", false);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Title", result.Title);
        Assert.Equal("Test Description", result.Description);
        Assert.Equal(TodoPriority.High, result.Priority);
        Assert.Equal(categoryId, result.CategoryId);
    }
    [Fact]
    public async Task GetTodoItemById_ShouldReturnTodo_WhenUserIsAdminOrManager()
    {
        // Arrange
        var (repoMock, _, service) = CreateServiceWithMocks();
        var todo = CreateTodo("Admin Test", "Visible to admin", TodoPriority.Low, "owner-user-id");
        repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
               .ReturnsAsync(todo);

        // Act
        var result = await service.GetTodoItemByIdAsync(Guid.NewGuid(), "different-user-id", true);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Admin Test", result.Title);
    }
}
