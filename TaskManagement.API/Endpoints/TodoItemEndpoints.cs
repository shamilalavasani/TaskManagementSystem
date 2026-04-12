using TaskManagement.Application.DTOs.QueryParametersDTOs;
using TaskManagement.Application.DTOs.TodoItemDTOs;
using TaskManagement.Application.Interfaces;
namespace TaskManagement.API.Endpoints;

public static class TodoItemEndpoints
{
    public static void MapTodoItemEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/todos").WithTags("TodoItems").RequireAuthorization();
        group.MapGet("/", GetAllTodoItems).AddEndpointFilter<ValidationFilter<TodoQueryParametersDto>>();
        group.MapGet("/{id:guid}", GetTodoItemById);
        group.MapPost("/", CreateTodoItem).AddEndpointFilter<ValidationFilter<CreateTodoItemDto>>();
        group.MapPut("/{id:guid}", UpdateTodoItem).AddEndpointFilter<ValidationFilter<UpdateTodoItemDto>>();
        group.MapDelete("/{id:guid}", DeleteTodoItem);
        group.MapGet("/overdue", GetOverdueTodoItems);
        group.MapGet("/due-next-7-days", GetTodoItemsDueInNext7Days);
        group.MapPatch("/{id:guid}/status", UpdateStatusTodoItem).AddEndpointFilter<ValidationFilter<UpdateTodoItemStatusDto>>();
    }
    private static async Task<IResult> GetAllTodoItems([AsParameters] TodoQueryParametersDto query, ITodoItemService service)
    {

        var items = await service.GetAllTodoItemsAsync(query);
        return Results.Ok(items);
    }

    private static async Task<IResult> GetTodoItemById(Guid id, ITodoItemService service)
    {
        var item = await service.GetTodoItemByIdAsync(id);

        return Results.Ok(item);
    }

    private static async Task<IResult> CreateTodoItem(CreateTodoItemDto dto, ITodoItemService service)
    {
        var createdItem = await service.CreateTodoItemAsync(dto);
        return Results.Created($"/todos/{createdItem.Id}", createdItem);
    }

    private static async Task<IResult> UpdateTodoItem(Guid id, UpdateTodoItemDto dto, ITodoItemService service)
    {

        await service.UpdateTodoItemAsync(id, dto);
        return Results.NoContent();
    }
    private static async Task<IResult> UpdateStatusTodoItem(Guid id, UpdateTodoItemStatusDto dto, ITodoItemService service)
    {


        await service.UpdateStatusTodoItemAsync(id, dto.Status);
        return Results.NoContent();

    }
    private static async Task<IResult> DeleteTodoItem(Guid id, ITodoItemService service)
    {
        await service.DeleteTodoItemAsync(id);
        return Results.NoContent();
    }
    private static async Task<IResult> GetOverdueTodoItems(ITodoItemService service)
    {
        var items = await service.GetOverdueTodoItemsAsync();
        return Results.Ok(items);
    }
    private static async Task<IResult> GetTodoItemsDueInNext7Days(ITodoItemService service)
    {
        var items = await service.GetTodoItemsDueInNext7DaysAsync();
        return Results.Ok(items);
    }
}