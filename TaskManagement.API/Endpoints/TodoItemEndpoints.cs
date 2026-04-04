using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;
namespace TaskManagement.API.Endpoints;

public static class TodoItemEndpoints
{
    public static void MapTodoItemEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/todos")
                       .WithTags("TodoItems");

        group.MapGet("/", GetAllTodoItems);
        group.MapGet("/{id:guid}", GetTodoItemById);
        group.MapPost("/", CreateTodoItem);
        group.MapPut("/{id:guid}", UpdateTodoItem);
        group.MapDelete("/{id:guid}", DeleteTodoItem);

        group.MapGet("/completed", GetCompletedTodoItems);
        group.MapGet("/pending", GetPendingTodoItems);
        group.MapGet("/overdue", GetOverdueTodoItems);
        group.MapGet("/due-next-7-days", GetTodoItemsDueInNext7Days);
        group.MapPatch("/{id:guid}/change-status", UpdateStatusTodoItem);
        group.MapGet("/cancelled", GetCancelledTodoItems);
        group.MapGet("/in-progress", GetInProgressTodoItems);

    }

    private static async Task<IResult> GetAllTodoItems(ITodoItemService service)
    {
        var items = await service.GetAllTodoItemsAsync();
        return Results.Ok(items);
    }

    private static async Task<IResult> GetTodoItemById(Guid id, ITodoItemService service)
    {
        var item = await service.GetTodoItemByIdAsync(id);
        //return item is null ? Results.NotFound() : Results.Ok(item); ba error handling middleware manage mishe  
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
        if (dto is null)
            throw new ArgumentException("Request body is required.");
        await service.UpdateStatusTodoItemAsync(id, dto.Status); // from body
        return Results.NoContent();

    }

    private static async Task<IResult> DeleteTodoItem(Guid id, ITodoItemService service)
    {

        await service.DeleteTodoItemAsync(id);
        return Results.NoContent();



    }

    private static async Task<IResult> GetCompletedTodoItems(ITodoItemService service)
    {
        var items = await service.GetCompletedTodoItemsAsync();
        return Results.Ok(items);
    }

    private static async Task<IResult> GetPendingTodoItems(ITodoItemService service)
    {
        var items = await service.GetPendingTodoItemsAsync();
        return Results.Ok(items);
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
    private static async Task<IResult> GetCancelledTodoItems(ITodoItemService service)
    {
        var items = await service.GetCancelledTodoItemsAsync();
        return Results.Ok(items);
    }
    private static async Task<IResult> GetInProgressTodoItems(ITodoItemService service)
    {
        var items = await service.GetInProgressTodoItemsAsync();
        return Results.Ok(items);
    }
}