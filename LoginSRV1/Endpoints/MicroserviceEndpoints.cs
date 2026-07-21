using LoginSRV1.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoginSRV1.Endpoints
{
    public static class MicroserviceEndpoints
    {
        public static void MapMicroserviceEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/microservices").WithTags("Microservices");

            // GET: /api/microservices - Obtener todas las URLs
            group.MapGet("/", async (IMicroserviceService service) =>
            {
                var config = service.GetConfig();
                return Results.Ok(config);
            })
            .WithName("GetMicroservices");

            // GET: /api/microservices/{name} - Obtener URL de un microservicio específico
            group.MapGet("/{name}", async (string name, IMicroserviceService service) =>
            {
                try
                {
                    var url = service.GetMicroserviceUrl(name);
                    return Results.Ok(new { name, url });
                }
                catch (ArgumentException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
            })
            .WithName("GetMicroserviceUrl");
        }
    }
}