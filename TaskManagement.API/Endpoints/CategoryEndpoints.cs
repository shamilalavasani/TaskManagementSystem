using TaskManagement.Application.DTOs.CategoryDTOs;
using TaskManagement.Application.DTOs.QueryParameters;
using TaskManagement.Application.Interfaces;
namespace TaskManagement.API.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/category").WithTags("Categories");
        group.MapGet("/", GetAllCategories).AddEndpointFilter<ValidationFilter<CategoryQueryParametersDto>>();
        group.MapGet("/{id:guid}", GetCategoryById);
        group.MapPost("/", CreateCategory).AddEndpointFilter<ValidationFilter<CreateCategoryDto>>();
        group.MapPut("/{id:guid}", UpdateCategory).AddEndpointFilter<ValidationFilter<UpdateCategoryDto>>();
        group.MapDelete("/{id:guid}", DeleteCategory);

    }

    private static async Task<IResult> DeleteCategory(Guid id, ICategoryService service)
    {
        await service.DeleteCategoryAsync(id);
        return Results.NoContent();
    }

    private static async Task<IResult> UpdateCategory(Guid id, UpdateCategoryDto dto, ICategoryService service)
    {

        await service.UpdateCategoryAsync(id, dto);
        return Results.NoContent();
    }

    private static async Task<IResult> CreateCategory(CreateCategoryDto dto, ICategoryService service)
    {
        var category = await service.CreateCategoryAsync(dto);
        return Results.Created($"/categories/{category.Id}", category);
    }

    private static async Task<IResult> GetCategoryById(Guid id, ICategoryService service)
    {
        var item = await service.GetCategoryByIdAsync(id);

        return Results.Ok(item);
    }

    private static async Task<IResult> GetAllCategories([AsParameters] CategoryQueryParametersDto query, ICategoryService service)
    {

        var items = await service.GetAllCategoriesAsync(query);
        return Results.Ok(items);
    }
}
