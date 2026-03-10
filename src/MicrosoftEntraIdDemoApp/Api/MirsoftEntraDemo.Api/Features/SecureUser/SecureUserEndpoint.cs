using SimpleCqrs;

namespace MirsoftEntraDemo.Api.Features.SecureUser;

public static class SecureUserEndpoint
{
    public static IEndpointRouteBuilder MapSecureUserEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/secure/user", async (
            HttpContext context,
            IAsyncQueryHandler<GetSecureUserQuery, GetSecureUserResponse> handler) =>
        {
            var query = new GetSecureUserQuery(context.User);
            var response = await handler.HandleAsync(query);
            return Results.Ok(response);
        })
        .RequireAuthorization()
        .WithName("GetSecureUser")
        .WithTags("SecureUser")
        .Produces<GetSecureUserResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized);

        return app;
    }
}