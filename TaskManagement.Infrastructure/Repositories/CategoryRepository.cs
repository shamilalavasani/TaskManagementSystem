using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.DTOs.CommonDTOs;
using TaskManagement.Application.DTOs.QueryParametersDTOs;
using TaskManagement.Application.Repositories;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Persistence.Context;


namespace TaskManagement.Infrastructure.Repositories;

public class CategoryRepository(AppDbContext context) : ICategoryRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Category> AddAsync(Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();
        return category;
    }

    public async Task DeleteAsync(Category category)
    {
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Categories.AnyAsync(x => x.Id == id);
    }

    public async Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null)
    {
        var normalizedName = name.Trim().ToLower();

        return await _context.Categories.AnyAsync(x =>
            x.Name.ToLower() == normalizedName &&
            (!excludeId.HasValue || x.Id != excludeId.Value));
    }

    public async Task<PagedResultDto<Category>> GetAllAsync(CategoryQueryParametersDto query)
    {

        IQueryable<Category> categoryQuery = _context.Categories.AsNoTracking().AsQueryable();



        // Search
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var searchTerm = query.Search.Trim().ToLower();

            categoryQuery = categoryQuery.Where(x =>
                x.Name.ToLower().Contains(searchTerm) ||
               (x.Description != null && x.Description.ToLower().Contains(searchTerm)));
        }

        // Sorting
        categoryQuery = (query.SortBy?.ToLower(), query.SortDirection?.ToLower()) switch
        {
            ("name", "asc") => categoryQuery.OrderBy(x => x.Name),
            ("name", "desc") => categoryQuery.OrderByDescending(x => x.Name),

            ("createdat", "asc") => categoryQuery.OrderBy(x => x.CreatedAt),
            ("createdat", "desc") => categoryQuery.OrderByDescending(x => x.CreatedAt),

            _ => categoryQuery.OrderByDescending(x => x.CreatedAt)
        };

        // Pagination
        var totalCount = await categoryQuery.CountAsync();
        var items = await categoryQuery
           .Skip((query.PageNumber - 1) * query.PageSize)
           .Take(query.PageSize)
           .ToListAsync();
        return new PagedResultDto<Category>
        {
            Items = items,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalCount = totalCount
        };
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        return await _context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task UpdateAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }
}
