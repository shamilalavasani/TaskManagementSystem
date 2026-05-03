using FluentAssertions;
using Moq;
using TaskManagement.Application.Common.Exceptions;
using TaskManagement.Application.DTOs.CommonDTOs;
using TaskManagement.Application.DTOs.QueryParametersDTOs;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Tests.Application.Services.CategoryServiceTest;

public class GetCategoryTests : CategoryServiceTestBase
{
    [Fact]
    public async Task GetCategoryByIdAsync_ShouldReturnCategory_WhenCategoryExists()
    {
        // Arrange
        var (repoMock, service) = CreateServiceWithMocks();

        var categoryId = Guid.NewGuid();
        var existingCategory = CreateCategory("Work", "Work tasks");

        repoMock
            .Setup(r => r.GetByIdAsync(categoryId))
            .ReturnsAsync(existingCategory);

        // Act
        var result = await service.GetCategoryByIdAsync(categoryId);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("Work");
        result.Description.Should().Be("Work tasks");

        repoMock.Verify(r => r.GetByIdAsync(categoryId), Times.Once);
    }

    [Fact]
    public async Task GetCategoryByIdAsync_ShouldThrowNotFoundException_WhenCategoryDoesNotExist()
    {
        // Arrange
        var (repoMock, service) = CreateServiceWithMocks();

        var categoryId = Guid.NewGuid();

        repoMock
            .Setup(r => r.GetByIdAsync(categoryId))
            .ReturnsAsync((Category?)null);

        // Act
        Func<Task> act = () => service.GetCategoryByIdAsync(categoryId);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task GetAllCategoriesAsync_ShouldReturnPagedCategories()
    {
        // Arrange
        var (repoMock, service) = CreateServiceWithMocks();

        var query = new CategoryQueryParametersDto();

        var categories = new List<Category>
        {
            CreateCategory("Work", "Work tasks"),
            CreateCategory("Personal", "Personal tasks")
        };

        var pagedResult = new PagedResultDto<Category>
        {
            Items = categories,
            PageNumber = 1,
            PageSize = 10,
            TotalCount = 2
        };

        repoMock
            .Setup(r => r.GetAllAsync(query))
            .ReturnsAsync(pagedResult);

        // Act
        var result = await service.GetAllCategoriesAsync(query);

        // Assert
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);

        repoMock.Verify(r => r.GetAllAsync(query), Times.Once);
    }
}