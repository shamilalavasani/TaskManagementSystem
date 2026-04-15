using System.Security.Claims;
using TaskManagement.API.Extensions;
using TaskManagement.Application.Common.Security;
using TaskManagement.Application.DTOs.QueryParametersDTOs;
using TaskManagement.Application.DTOs.TodoItemDTOs;
using TaskManagement.Application.Interfaces;
namespace TaskManagement.API.Endpoints;

public static class TodoItemEndpoints
{
    public static void MapTodoItemEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/todos").WithTags("TodoItems").RequireAuthorization(AppPolicies.UserOrAbove);
        group.MapGet("/", GetAllTodoItems).AddEndpointFilter<ValidationFilter<TodoQueryParametersDto>>();
        group.MapGet("/{id:guid}", GetTodoItemById);
        group.MapPost("/", CreateTodoItem).AddEndpointFilter<ValidationFilter<CreateTodoItemDto>>();
        group.MapPut("/{id:guid}", UpdateTodoItem).AddEndpointFilter<ValidationFilter<UpdateTodoItemDto>>();
        group.MapDelete("/{id:guid}", DeleteTodoItem);
        group.MapGet("/overdue", GetOverdueTodoItems);
        group.MapGet("/due-next-7-days", GetTodoItemsDueInNext7Days);
        group.MapPatch("/{id:guid}/status", UpdateStatusTodoItem).AddEndpointFilter<ValidationFilter<UpdateTodoItemStatusDto>>();
    }
    private static async Task<IResult> GetAllTodoItems([AsParameters] TodoQueryParametersDto query, ITodoItemService service, ClaimsPrincipal user)
    {
        var userContext = user.GetUserContext();
        if (userContext is null)
            return Results.Unauthorized();

        var (userId, isAdminOrManager) = userContext.Value;

        var items = await service.GetAllTodoItemsAsync(query, userId, isAdminOrManager);
        return Results.Ok(items);
    }

    private static async Task<IResult> GetTodoItemById(Guid id, ITodoItemService service, ClaimsPrincipal user)
    {
        var userContext = user.GetUserContext();
        if (userContext is null)
            return Results.Unauthorized();

        var (userId, isAdminOrManager) = userContext.Value;
        var item = await service.GetTodoItemByIdAsync(id, userId, isAdminOrManager);

        return Results.Ok(item);
    }

    private static async Task<IResult> CreateTodoItem(CreateTodoItemDto dto, ITodoItemService service, ClaimsPrincipal user)
    {
        var userContext = user.GetUserContext();
        if (userContext is null)
            return Results.Unauthorized();

        var (userId, _) = userContext.Value;
        var createdItem = await service.CreateTodoItemAsync(dto, userId);
        return Results.Created($"/todos/{createdItem.Id}", createdItem);
    }

    private static async Task<IResult> UpdateTodoItem(Guid id, UpdateTodoItemDto dto, ITodoItemService service, ClaimsPrincipal user)
    {
        var userContext = user.GetUserContext();
        if (userContext is null)
            return Results.Unauthorized();

        var (userId, isAdminOrManager) = userContext.Value;

        await service.UpdateTodoItemAsync(id, dto, userId, isAdminOrManager);
        return Results.NoContent();
    }
    private static async Task<IResult> UpdateStatusTodoItem(Guid id, UpdateTodoItemStatusDto dto, ITodoItemService service, ClaimsPrincipal user)
    {
        var userContext = user.GetUserContext();
        if (userContext is null)
            return Results.Unauthorized();

        var (userId, isAdminOrManager) = userContext.Value;


        await service.UpdateStatusTodoItemAsync(id, dto.Status, userId, isAdminOrManager);
        return Results.NoContent();

    }
    private static async Task<IResult> DeleteTodoItem(Guid id, ITodoItemService service, ClaimsPrincipal user)
    {
        var userContext = user.GetUserContext();
        if (userContext is null)
            return Results.Unauthorized();

        var (userId, isAdminOrManager) = userContext.Value;

        await service.DeleteTodoItemAsync(id, userId, isAdminOrManager);
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