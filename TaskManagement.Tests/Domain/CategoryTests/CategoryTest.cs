using FluentAssertions;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Tests.Domain.CategoryTests;

public class CategoryTest
{

    [Fact]
    public void Update_ShouldThrowException_WhenNameIsEmpty()
    {
        // Arrange
        var category = CreateCategory();

        // Act
        Action act = () => category.Update("", "New Desc");

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Update_ShouldChangeNameAndDescription_WhenValid()
    {
        // Arrange
        var category = CreateCategory();

        // Act
        category.Update("New Name", "New Desc");

        // Assert
        category.Name.Should().Be("New Name");
        category.Description.Should().Be("New Desc");
    }
    [Fact]
    public void Update_ShouldThrowException_WhenNameIsWhitespace()
    {
        // Arrange
        var category = CreateCategory();

        // Act
        Action act = () => category.Update("   ", "Desc");

        // Assert
        act.Should().Throw<ArgumentException>();
    }
    private static Category CreateCategory()
    {
        return new Category("Test Category", "Test Description");
    }
}
