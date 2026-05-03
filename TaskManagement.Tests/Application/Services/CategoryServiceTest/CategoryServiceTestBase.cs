using Moq;
using TaskManagement.Application.Repositories;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Tests.Application.Services.CategoryServiceTest;

public abstract class CategoryServiceTestBase
{
    protected static Category CreateCategory(
        string name = "Test Category",
        string? description = "Test Description")
    {
        return new Category(name, description);
    }

    protected static (Mock<ICategoryRepository> repoMock, CategoryService service) CreateServiceWithMocks()
    {
        var repoMock = new Mock<ICategoryRepository>();
        var service = new CategoryService(repoMock.Object);

        return (repoMock, service);
    }
}