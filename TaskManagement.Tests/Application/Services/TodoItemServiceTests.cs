using Moq;
using TaskManagement.Application.Common.Exceptions;
using TaskManagement.Application.Repositories;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Tests.Application.Services;

public class TodoItemServiceTests
{
    [Fact]
    public async Task GetTodoItemById_ShouldThrowNotFound_WhenTodoDoesNotExist()
    {
        // Arrange
        var repoMock = new Mock<ITodoItemRepository>();
        var categoryMock = new Mock<ICategoryRepository>(); // because the service constructor requires it, even if not used in this test

        repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((TodoItem?)null);// Simulate not found /((TodoItem)null!)

        var service = new TodoItemService(repoMock.Object, categoryMock.Object);//sut(subject under test)

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            service.GetTodoItemByIdAsync(Guid.NewGuid(), "user1", false));
    }
}
