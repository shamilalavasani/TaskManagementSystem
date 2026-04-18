using Moq;
using TaskManagement.Application.Repositories;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using AppService = TaskManagement.Application.Services;

namespace TaskManagement.Tests.Application.Services.TodoItemServiceTest;


public abstract class TodoItemServiceTestBase
{
    protected TodoItem CreateTodo(string title, string description, TodoPriority priority,
        string ownerId = "owner-user-id",
        Guid? categoryId = null)
    {
        return new TodoItem(
            title,
            description,
            DateTime.UtcNow.AddDays(1),
            categoryId ?? Guid.NewGuid(),
            ownerId,
            priority);
    }

    protected (Mock<ITodoItemRepository>, Mock<ICategoryRepository>, AppService.TodoItemService) CreateServiceWithMocks()
    {
        var repoMock = new Mock<ITodoItemRepository>();
        var categoryMock = new Mock<ICategoryRepository>();
        // constructor requires both repositories, but category repository is not used in this test, so we can just create a mock without setup
        var service = new AppService.TodoItemService(repoMock.Object, categoryMock.Object);

        return (repoMock, categoryMock, service);
    }
}
