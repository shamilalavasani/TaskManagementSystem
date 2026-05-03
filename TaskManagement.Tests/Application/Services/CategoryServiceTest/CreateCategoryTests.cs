using FluentAssertions;
using Moq;
using TaskManagement.Application.DTOs.CategoryDTOs;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Tests.Application.Services.CategoryServiceTest;

public class CreateCategoryTests : CategoryServiceTestBase
{
    [Fact]
    public async Task CreateCategoryAsync_ShouldCreateCategory_WhenRequestIsValid()
    {
        // Arrange
        var (repoMock, service) = CreateServiceWithMocks();

        var createDto = new CreateCategoryDto
        {
            Name = "Work",
            Description = "Work related tasks"
        };

        repoMock
            .Setup(r => r.ExistsByNameAsync(createDto.Name, null))
            .ReturnsAsync(false);

        repoMock
            .Setup(r => r.AddAsync(It.IsAny<Category>()))
            .ReturnsAsync((Category category) => category);

        // Act
        var result = await service.CreateCategoryAsync(createDto);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(createDto.Name);
        result.Description.Should().Be(createDto.Description);

        repoMock.Verify(r => r.ExistsByNameAsync(createDto.Name, null), Times.Once);
        repoMock.Verify(r => r.AddAsync(It.Is<Category>(c =>
            c.Name == createDto.Name &&
            c.Description == createDto.Description
        )), Times.Once);
    }
}