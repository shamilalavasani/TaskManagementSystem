using FluentAssertions;
using Moq;
using TaskManagement.Application.Common.Exceptions;
using TaskManagement.Application.DTOs.TodoItemDTOs;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
namespace TaskManagement.Tests.Application.Services.TodoItemServiceTest;

public class CreateTodoItemTests : TodoItemServiceTestBase
{


    [Fact]
    public async Task CreateTodoItemAsync_Should_Create_And_Return_TodoItemDto_When_Input_Is_Valid()
    {
        // Arrange
        var (repoMock, categoryMock, service) = CreateServiceWithMocks();
        var userId = "test-user-id";
        var categoryId = Guid.NewGuid(); ;

        var createDto = new CreateTodoItemDto
        {
            Title = "Test Title",
            Description = "Test Description",
            DueDate = DateTime.UtcNow.AddDays(2),
            CategoryId = categoryId,
            Priority = TodoPriority.Medium
        };

        var category = new Category("Work");

        categoryMock
            .Setup(x => x.GetByIdAsync(createDto.CategoryId))
            .ReturnsAsync((Category?)category);

        repoMock
            .Setup(x => x.AddAsync(It.IsAny<TodoItem>()))
            .ReturnsAsync((TodoItem todoItem) => todoItem);

        // Act
        var result = await service.CreateTodoItemAsync(createDto, userId);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be(createDto.Title);
        result.Description.Should().Be(createDto.Description);
        result.DueDate.Should().Be(createDto.DueDate);
        result.CategoryId.Should().Be(createDto.CategoryId);
        result.Priority.Should().Be(createDto.Priority);

        repoMock.Verify(x => x.AddAsync(It.IsAny<TodoItem>()), Times.Once);
    }

    [Fact]
    public async Task CreateTodoItemAsync_Should_Throw_Exception_When_Category_Does_Not_Exist()
    {
        // Arrange
        var (repoMock, categoryMock, service) = CreateServiceWithMocks();
        var userId = "test-user-id";
        var createDto = new CreateTodoItemDto
        {
            Title = "Test Title",
            Description = "Test Description",
            DueDate = DateTime.UtcNow.AddDays(2),
            CategoryId = Guid.NewGuid(),
            Priority = TodoPriority.High
        };

        categoryMock.Setup(x => x.GetByIdAsync(createDto.CategoryId))
            .ReturnsAsync((Category?)null);

        // Act
        Func<Task> act = async () => await service.CreateTodoItemAsync(createDto, userId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage("*Category*");

        repoMock.Verify(x => x.AddAsync(It.IsAny<TodoItem>()), Times.Never);
    }
    [Fact]
    public async Task CreateTodoItemAsync_Should_Pass_Correct_TodoItem_To_Repository()
    {
        // Arrange
        var (repoMock, categoryMock, service) = CreateServiceWithMocks();
        var userId = "test-user-id";
        var categoryId = Guid.NewGuid();

        var createDto = new CreateTodoItemDto
        {
            Title = "New Task",
            Description = "Task Description",
            DueDate = DateTime.UtcNow.AddDays(3),
            CategoryId = categoryId,
            Priority = TodoPriority.Low
        };

        categoryMock
            .Setup(x => x.GetByIdAsync(categoryId))
            .ReturnsAsync((Category?)new Category("Personal"));

        repoMock
            .Setup(x => x.AddAsync(It.IsAny<TodoItem>()))
            .ReturnsAsync((TodoItem todoItem) => todoItem);

        // Act
        await service.CreateTodoItemAsync(createDto, userId);

        // Assert
        repoMock.Verify(x => x.AddAsync(It.Is<TodoItem>(t =>
            t.Title == createDto.Title &&
            t.Description == createDto.Description &&
            t.DueDate == createDto.DueDate &&
            t.CategoryId == createDto.CategoryId &&
            t.Priority == createDto.Priority
        )), Times.Once);
    }
    [Fact]
    public async Task CreateTodoItemAsync_Should_Return_Dto_With_Default_Status()
    {
        // Arrange
        var (repoMock, categoryMock, service) = CreateServiceWithMocks();
        var userId = "test-user-id";
        var categoryId = Guid.NewGuid();

        var createDto = new CreateTodoItemDto
        {
            Title = "Task A",
            Description = "Desc A",
            DueDate = DateTime.UtcNow.AddDays(1),
            CategoryId = categoryId,
            Priority = TodoPriority.Medium
        };

        categoryMock
            .Setup(x => x.GetByIdAsync(categoryId))
            .ReturnsAsync(new Category("Work"));

        repoMock
            .Setup(x => x.AddAsync(It.IsAny<TodoItem>()))
            .ReturnsAsync((TodoItem todoItem) => todoItem);

        // Act
        var result = await service.CreateTodoItemAsync(createDto, userId);

        // Assert
        result.CompletionStatus.Should().Be(TodoItemStatus.Pending);
        result.Id.Should().NotBe(Guid.Empty);
        result.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }
}
