using Moq;
using TaskManagement.Application.Common.Exceptions;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Tests.Application.Services.TodoItemServiceTest;

public class DeleteTodoItemTests : TodoItemServiceTestBase
{
    [Fact]
    public async Task DeleteTodoItem_ShouldThrowNotFound_WhenTodoDoesNotExist()
    {
        // Arrange

        var (repoMock, _, service) = CreateServiceWithMocks();
        repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((TodoItem?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.DeleteTodoItemAsync(Guid.NewGuid(), "user1", false));
    }
    [Fact]
    public async Task DeleteTodoItem_ShouldThrowForbidden_WhenUserIsNotOwner()
    {
        // Arrange
        var (repoMock, _, service) = CreateServiceWithMocks();
        var todo = CreateTodo("Delete Test", "Delete Description", TodoPriority.Medium, "owner-user-id");

        repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(todo);


        // Act & Assert
        await Assert.ThrowsAsync<ForbiddenException>(() =>
         service.DeleteTodoItemAsync(Guid.NewGuid(), "other-user-id", false));
    }
    [Fact]
    public async Task DeleteTodoItem_ShouldCallDeleteAsync_WhenUserIsOwner()
    {
        // Arrange
        var (repoMock, _, service) = CreateServiceWithMocks();
        var todo = CreateTodo("Delete Test", "Delete Description", TodoPriority.Medium, "owner-user-id");

        repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(todo);

        // Act
        await service.DeleteTodoItemAsync(Guid.NewGuid(), "owner-user-id", false);

        // Assert
        repoMock.Verify(r => r.DeleteAsync(todo), Times.Once);// delete didnt return anything, so we verify that it was called once with the correct todo item
    }
    [Fact]
    public async Task DeleteTodoItem_ShouldCallDeleteAsync_WhenUserIsAdminOrManager()
    {
        // Arrange
        var (repoMock, _, service) = CreateServiceWithMocks();

        var todo = CreateTodo("Delete Test", "Delete Description", TodoPriority.Medium, "owner-user-id");

        repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(todo);

        // Act
        await service.DeleteTodoItemAsync(Guid.NewGuid(), "another-user-id", true);

        // Assert
        repoMock.Verify(r => r.DeleteAsync(todo), Times.Once);
    }


}
