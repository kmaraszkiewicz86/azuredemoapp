using System.Security.Claims;
using SimpleCqrs;

namespace MirsoftEntraDemo.Api.Features.SecureUser;

public sealed record GetSecureUserQuery(ClaimsPrincipal User) : IQuery<GetSecureUserResponse>;

public sealed record GetSecureUserResponse(bool IsLogged, string UserName, string? Email);

public sealed class GetSecureUserQueryHandler : IAsyncQueryHandler<GetSecureUserQuery, GetSecureUserResponse>
{
    public Task<GetSecureUserResponse> HandleAsync(GetSecureUserQuery query, CancellationToken cancellationToken = default)
    {
        var user = query.User;
        var isLogged = user.Identity?.IsAuthenticated ?? false;
        var userName = user.Identity?.Name ?? "Unknown User";
        var email = user.FindFirst("preferred_username")?.Value
                 ?? user.FindFirst("email")?.Value;

        return Task.FromResult(new GetSecureUserResponse(isLogged, userName, email));
    }
}