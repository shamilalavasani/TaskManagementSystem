using FluentAssertions;
using Moq;
using TaskManagement.Application.Common.Exceptions;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Tests.Application.Services.CategoryServiceTest;

public class DeleteCategoryTests : CategoryServiceTestBase
{
    [Fact]
    public async Task DeleteCategoryAsync_ShouldDeleteCategory_WhenCategoryExists()
    {
        // Arrange
        var (repoMock, service) = CreateServiceWithMocks();

        var categoryId = Guid.NewGuid();
        var existingCategory = CreateCategory("Work", "Work tasks");

        repoMock
            .Setup(r => r.GetByIdAsync(categoryId))
            .ReturnsAsync(existingCategory);

        // Act
        await service.DeleteCategoryAsync(categoryId);

        // Assert
        repoMock.Verify(r => r.DeleteAsync(existingCategory), Times.Once);
    }

    [Fact]
    public async Task DeleteCategoryAsync_ShouldThrowNotFoundException_WhenCategoryDoesNotExist()
    {
        // Arrange
        var (repoMock, service) = CreateServiceWithMocks();

        var categoryId = Guid.NewGuid();

        repoMock
            .Setup(r => r.GetByIdAsync(categoryId))
            .ReturnsAsync((Category?)null);

        // Act
        Func<Task> act = () => service.DeleteCategoryAsync(categoryId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();

        repoMock.Verify(r => r.DeleteAsync(It.IsAny<Category>()), Times.Never);
    }

}