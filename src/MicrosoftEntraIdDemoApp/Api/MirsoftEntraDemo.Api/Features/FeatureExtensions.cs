using MirsoftEntraDemo.Api.Features.SecureUser;

namespace MirsoftEntraDemo.Api.Features;

public static class FeatureExtensions
{
    public static IEndpointRouteBuilder MapFeatureEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapSecureUserEndpoint();
        return app;
    }
}