using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using NomNom.Infrastructure.Data;

namespace NomNomApi.Functions;

public class HealthCheckFunction(RecipesDbContext context)
{
    [Function("health")]
    public async Task<HttpResponseData> HealthCheck(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData request)
    {
        try
        {
            await context.Database.CanConnectAsync();

            var response = request.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(new { status = "healthy", timestamp = DateTime.UtcNow });

            return response;
        }
        catch (Exception ex)
        {
            var response = request.CreateResponse(HttpStatusCode.ServiceUnavailable);
            await response.WriteAsJsonAsync(new { status = "unhealthy", error = ex.Message });

            return response;
        }
    }
}
