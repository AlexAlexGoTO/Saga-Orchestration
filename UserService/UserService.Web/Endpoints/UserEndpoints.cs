using MediatR;
using UserService.Application.Common.Dto;
using UserService.Application.Users.Commands;
using UserService.Application.Users.Queries;

namespace UserService.Web.Endpoints;
public static class UserEndpoints
{
    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/users");

        group.MapGet("/", async (HttpContext context, IMediator mediator) =>
        {
            var result = await mediator.Send(new GetUsersQuery());

            return Results.Ok(result);
        });

        group.MapPost("/", async (UserDto user, HttpContext context, IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateUserCommand
            {
                Name = user.Name
            });

            return Results.Ok();
        });
    }
}
