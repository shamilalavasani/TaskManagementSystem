using FluentAssertions;
using Moq;
using TaskManagement.Application.Common.Exceptions;
using TaskManagement.Application.DTOs.CategoryDTOs;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Tests.Application.Services.CategoryServiceTest;

public class UpdateCategoryTests : CategoryServiceTestBase
{
    [Fact]
    public async Task UpdateCategoryAsync_ShouldUpdateCategory_WhenValid()
    {
        // Arrange
        var (repoMock, service) = CreateServiceWithMocks();

        var categoryId = Guid.NewGuid();

        var existingCategory = CreateCategory("Old Name", "Old Desc");

        var updateDto = new UpdateCategoryDto
        {
            Name = "New Name",
            Description = "New Desc"
        };

        repoMock
            .Setup(r => r.GetByIdAsync(categoryId))
            .ReturnsAsync(existingCategory);

        repoMock
            .Setup(r => r.ExistsByNameAsync(updateDto.Name, categoryId))
            .ReturnsAsync(false);

        // Act
        await service.UpdateCategoryAsync(categoryId, updateDto);

        // Assert
        existingCategory.Name.Should().Be("New Name");
        existingCategory.Description.Should().Be("New Desc");

        repoMock.Verify(r => r.UpdateAsync(existingCategory), Times.Once);
    }
    [Fact]
    public async Task UpdateCategoryAsync_ShouldThrowNotFoundException_WhenCategoryDoesNotExist()
    {
        // Arrange
        var (repoMock, service) = CreateServiceWithMocks();

        var categoryId = Guid.NewGuid();

        var updateDto = new UpdateCategoryDto
        {
            Name = "New Name",
            Description = "New Desc"
        };

        repoMock
            .Setup(r => r.GetByIdAsync(categoryId))
            .ReturnsAsync((Category?)null);

        // Act
        Func<Task> act = () => service.UpdateCategoryAsync(categoryId, updateDto);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();

        repoMock.Verify(r => r.UpdateAsync(It.IsAny<Category>()), Times.Never);
    }
    [Fact]
    public async Task UpdateCategoryAsync_ShouldThrowException_WhenNameAlreadyExists()
    {
        // Arrange
        var (repoMock, service) = CreateServiceWithMocks();

        var categoryId = Guid.NewGuid();

        var existingCategory = CreateCategory("Old", "Desc");

        var updateDto = new UpdateCategoryDto
        {
            Name = "Duplicate",
            Description = "New Desc"
        };

        repoMock
            .Setup(r => r.GetByIdAsync(categoryId))
            .ReturnsAsync(existingCategory);

        repoMock
            .Setup(r => r.ExistsByNameAsync(updateDto.Name, categoryId))
            .ReturnsAsync(true);

        // Act
        Func<Task> act = () => service.UpdateCategoryAsync(categoryId, updateDto);

        // Assert
        await act.Should().ThrowAsync<BadRequestException>();

        repoMock.Verify(r => r.UpdateAsync(It.IsAny<Category>()), Times.Never);
    }
}
