using TaskManagement.Application.DTOs.CommonDTOs;
using TaskManagement.Application.DTOs.QueryParametersDTOs;
using TaskManagement.Domain.Entities;
namespace TaskManagement.Application.Repositories;

public interface ICategoryRepository
{
    Task<Category> AddAsync(Category category);
    Task<Category?> GetByIdAsync(Guid id);
    Task<PagedResultDto<Category>> GetAllAsync(CategoryQueryParametersDto query);
    Task UpdateAsync(Category category);
    Task DeleteAsync(Category category);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null);
}
