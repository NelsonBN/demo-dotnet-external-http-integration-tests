using System.Threading;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Demo.Api;

public static class BlogEndpoints
{
    public static void MapProductsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/comments", async (JsonPlaceholderService service, CancellationToken cancellationToken) =>
        {
            var comments = await service.GetCommentsAsync(cancellationToken);
            return Results.Ok(comments);
        }).WithOpenApi();

        endpoints.MapGet("/comments/{id:int}", async (JsonPlaceholderService service, int id, CancellationToken cancellationToken) =>
        {
            var comment = await service.GetCommentAsync(id, cancellationToken);
            return Results.Ok(comment);
        }).WithOpenApi();
    }
}
